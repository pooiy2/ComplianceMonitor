using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using global::Backend.Enums;
using global::Backend.Models;
using global::Backend.Repository;
using global::Backend.Services;
using global::Backend.ViewModels;
using Xunit;

namespace Backend.Test
{
    public class HistoryServiceTests
    {
        class FakeHistoryRepository : IHistoryRepository
        {
            private readonly List<History> _items;

            public FakeHistoryRepository(List<History> items)
            {
                _items = items;
            }

            public Task<History> Add(History item)
            {
                _items.Add(item);
                return Task.FromResult(item);
            }

            public Task<List<History>> GetAll()
            {
                return Task.FromResult(new List<History>(_items));
            }
        }

        [Fact]
        public async Task GetHistory_MapsPropertiesAndResultToLowercase()
        {
            var items = new List<History>
            {
                new History { Id = 1, Action = "act", Guideline = "g", Confidence = 0.9, Timestamp = DateTime.UtcNow, Result = Result.COMPLIES }
            };

            var repo = new FakeHistoryRepository(items);
            var svc = new HistoryService(repo);

            var result = await svc.GetHistory();

            Assert.Single(result);
            var vm = result[0];
            Assert.Equal(1, vm.Id);
            Assert.Equal("act", vm.Action);
            Assert.Equal("g", vm.Guideline);
            Assert.Equal(0.9, vm.Confidence);
            Assert.Equal("complies", vm.Result);
        }

        [Fact]
        public async Task GetSummary_ReturnsCorrectCounts()
        {
            var items = new List<History>
            {
                new History { Result = Result.COMPLIES },
                new History { Result = Result.COMPLIES },
                new History { Result = Result.DEVIATES },
                new History { Result = Result.UNCLEAR }
            };

            var repo = new FakeHistoryRepository(items);
            var svc = new HistoryService(repo);

            var summary = await svc.GetSummary();

            Assert.Equal(4, summary.Total);
            Assert.Equal(2, summary.Complies);
            Assert.Equal(1, summary.Deviates);
            Assert.Equal(1, summary.Unclear);
        }
    }
}
