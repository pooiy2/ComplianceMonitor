using System;
using Xunit;
using Backend.Repository;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Backend.Test
{
    public class HistoryRepositoryTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Add_PersistsEntity_And_GetAllReturnsIt()
        {
            using var ctx = CreateInMemoryContext();
            var repo = new HistoryRepository(ctx);

            var h = new History { Action = "a", Guideline = "g", Confidence = 0.5, Timestamp = DateTime.UtcNow, Result = Backend.Enums.Result.COMPLIES };
            await repo.Add(h);

            var list = await repo.GetAll();
            Assert.Single(list);
            Assert.Equal("a", list.First().Action);
            Assert.Equal(Backend.Enums.Result.COMPLIES, list.First().Result);
        }
    }
}
