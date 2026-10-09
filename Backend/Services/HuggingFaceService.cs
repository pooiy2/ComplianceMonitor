namespace Backend.Services
{
    public interface IHuggingFaceService
    {
        Task<string> AnalyzeTextAsync(string inputText, string guideline);
    }
}
