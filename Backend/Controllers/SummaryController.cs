using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.ViewModels;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        IHistoryService _historyService;

        public SummaryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost]
        public async Task<IActionResult> GetSummary([FromBody] string input)
        {
            SummaryViewModel summary = await _historyService.GetSummary();
            return Ok(summary);
        }

    }
}