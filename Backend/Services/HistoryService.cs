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
        Task<SummaryViewModel> GetSummary();
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

        public async Task<SummaryViewModel> GetSummary()
        {
            List<History> history = await _historyRepository.GetAll();
            return new SummaryViewModel
            {
                Total = history.Count,
                Complies = history.Count(h => h.Result == Result.COMPLIES),
                Deviates = history.Count(h => h.Result == Result.DEVIATES),
                Unclear = history.Count(h => h.Result == Result.UNCLEAR)
            };
        }
    }
}
