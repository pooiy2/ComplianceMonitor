using System.Globalization;
using Backend.Enums;
using Backend.Models;
using Backend.Repository;
using Backend.ViewModels;

namespace Backend.Services
{

    public interface IHistoryService
    {
        Task<List<HistoryViewModel>> GetHistory();
        Task<string> GetSummary();
    }

    public class HistoryService : IHistoryService
    {
        IHistoryRepository _historyRepository;
        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<List<HistoryViewModel>> GetHistory()
        {
            List<History> history = await _historyRepository.GetAll();
            return history.Select(h => new HistoryViewModel
            {
                Id = h.Id,
                Action = h.Action,
                Guideline = h.Guideline,
                Confidence = h.Confidence,
                Timestamp = h.Timestamp,
                Result = h.Result?.ToString()?.ToLower() ?? ""
            }).ToList();
        }

        public async Task<string> GetSummary()
        {
            // Implement logic to retrieve summary from the database or any other source
            return await Task.FromResult("Summary data");
        }
    }
}
