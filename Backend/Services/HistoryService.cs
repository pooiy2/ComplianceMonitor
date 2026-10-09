namespace Backend.Services
{

    public interface IHistoryService
    {
        Task<string> GetHistory();
        Task<string> GetSummary();
    }
}
