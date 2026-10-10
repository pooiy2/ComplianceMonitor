using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Backend.Services;
using Backend.ViewModels;
using Backend.Models;
using Backend.Repository;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Backend.Test
{
    // Simple HttpMessageHandler stub to return a pre-defined HttpResponseMessage
    class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public StubHttpMessageHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(_response);
    }

    class FakeHistoryRepository : IHistoryRepository
    {
        public List<History> Items { get; } = new();
        public Task<History> Add(History item) { Items.Add(item); return Task.FromResult(item); }
        public Task<List<History>> GetAll() => Task.FromResult(new List<History>(Items));
    }

    public class HuggingFaceServiceTests
    {
        IConfiguration BuildConfig(string token) => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["HUGGINGFACE_API_TOKEN"] = token })
            .Build();

        [Fact]
        public async Task AnalyzeTextAsync_Throws_WhenTokenMissing()
        {
            var http = new HttpClient(new StubHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)));
            var cfg = BuildConfig(string.Empty);
            var repo = new FakeHistoryRepository();
            var service = new HuggingFaceService(http, cfg, repo);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AnalyzeTextAsync(new HuggingFaceRequestViewModel { Action = "a", Guideline = "g" }));
        }

        [Fact]
        public async Task AnalyzeTextAsync_ThrowsOnNonSuccessResponse()
        {
            var content = new StringContent("error text", Encoding.UTF8, "application/json");
            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = content };
            var http = new HttpClient(new StubHttpMessageHandler(resp));
            var cfg = BuildConfig("token");
            var repo = new FakeHistoryRepository();
            var service = new HuggingFaceService(http, cfg, repo);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AnalyzeTextAsync(new HuggingFaceRequestViewModel { Action = "x", Guideline = "y" }));

            Assert.Contains("Hugging Face API request failed", ex.Message);
        }

        [Fact]
        public async Task AnalyzeTextAsync_ParsesArrayResponse_StoresHistoryAndReturnsTop()
        {
            // two candidates, first with lower score, second higher => second is top
            var json = "[{\"label\":\"complies\",\"score\":0.3},{\"label\":\"deviates\",\"score\":0.8}]";
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var http = new HttpClient(new StubHttpMessageHandler(resp));
            var cfg = BuildConfig("token");
            var repo = new FakeHistoryRepository();
            var service = new HuggingFaceService(http, cfg, repo);

            var vm = await service.AnalyzeTextAsync(new HuggingFaceRequestViewModel { Action = "act", Guideline = "g" });

            // returned top label and score
            Assert.Equal("deviates", vm.Result.ToLower());
            Assert.Equal(0.8, vm.Confidence, 3);

            // repository persisted item with parsed Result (DEVIATES)
            Assert.Single(repo.Items);
            Assert.Equal(0.8, repo.Items[0].Confidence);
            Assert.Equal(Backend.Enums.Result.DEVIATES, repo.Items[0].Result);
        }

        [Fact]
        public async Task AnalyzeTextAsync_NonArrayResponse_ResultsInUnknownAndUnclearHistory()
        {
            var json = "{\"label\":\"complies\",\"score\":0.9}"; // object, not array
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var http = new HttpClient(new StubHttpMessageHandler(resp));
            var cfg = BuildConfig("token");
            var repo = new FakeHistoryRepository();
            var service = new HuggingFaceService(http, cfg, repo);

            var vm = await service.AnalyzeTextAsync(new HuggingFaceRequestViewModel { Action = "a", Guideline = "g" });

            // service currently returns "UNKNOWN" and repository stores UNCLEAR
            Assert.Equal("UNKNOWN", vm.Result);
            Assert.Single(repo.Items);
            Assert.Equal(Backend.Enums.Result.UNCLEAR, repo.Items[0].Result);
        }
    }
}
