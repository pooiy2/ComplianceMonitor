using System.Globalization;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public interface IHistoryRepository
    {
        Task<List<History>> GetAll();
        Task<History> Add(History item);
    }

    public class HistoryRepository : IHistoryRepository
    {
        private readonly AppDbContext _context;

        public HistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<History> Add(History item)
        {
            _context.Histories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<History>> GetAll()
        {
            return await _context.Histories
                .AsNoTracking()
                .ToListAsync();
        }
    }
}