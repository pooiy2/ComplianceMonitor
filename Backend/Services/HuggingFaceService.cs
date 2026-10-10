using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json;
using Backend.Enums;
using Backend.Models;
using Backend.Repository;
using Backend.ViewModels;

namespace Backend.Services
{
    public interface IHuggingFaceService
    {
        Task<HuggingFaceResponseViewModel> AnalyzeTextAsync(HuggingFaceRequestViewModel request);
    }

    public class HuggingFaceService : IHuggingFaceService
    {
        private const string Endpoint = "https://router.huggingface.co/hf-inference/models/facebook/bart-large-mnli";

        private readonly HttpClient _httpClient;
        private readonly string _bearerToken;
        private readonly IHistoryRepository _historyRepository;

        public HuggingFaceService(HttpClient httpClient, IConfiguration configuration, IHistoryRepository historyRepository)
        {
            _httpClient = httpClient;
            _bearerToken = configuration["HUGGINGFACE_API_TOKEN"];
            _historyRepository = historyRepository;
            if (!string.IsNullOrEmpty(_bearerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            }
        }

        public async Task<HuggingFaceResponseViewModel> AnalyzeTextAsync(HuggingFaceRequestViewModel request)
        {
            string? inputText = request?.Action;
            string? guideline = request?.Guideline;
            if (inputText == null) inputText = string.Empty;
            if (guideline == null) guideline = string.Empty;

            if (string.IsNullOrWhiteSpace(_bearerToken))
            {
                throw new InvalidOperationException("Hugging Face API token not found. Set the HUGGINGFACE_API_TOKEN environment variable.");
            }

            var payload = new
            {
                inputs = $"Action: {inputText} Guideline: {guideline}",
                parameters = new
                {
                    candidate_labels = new[] { "complies", "deviates", "unclear" }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Endpoint, content).ConfigureAwait(false);

            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Hugging Face API request failed ({response.StatusCode}): {responseText}");
            }

            try
            {
                using var doc = JsonDocument.Parse(responseText);

                string topLabel = "UNKNOWN";
                double topScore = 0.0;

                // Handle responses that are either an object or an array of objects
                JsonElement root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                {
                    var highestScoreElement = root.EnumerateArray().OrderByDescending(e => e.GetProperty("score").GetDouble()).FirstOrDefault();
                    topLabel = highestScoreElement.GetProperty("label").GetString() ?? "UNKNOWN";
                    topScore = highestScoreElement.GetProperty("score").GetDouble();
                }

                History history = new History
                {
                    Action = inputText,
                    Guideline = guideline,
                    Result = Enum.TryParse(topLabel.ToUpper(), out Result parsedResult) ? parsedResult : Result.UNCLEAR,
                    Confidence = topScore,
                    Timestamp = DateTime.UtcNow
                };

                await _historyRepository.Add(history);

                return new HuggingFaceResponseViewModel
                {
                    Action = inputText,
                    Guideline = guideline,
                    Result = topLabel,
                    Confidence = topScore,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse Hugging Face response.", ex);
            }
        }
    }
}
