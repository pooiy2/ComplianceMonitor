using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public SummaryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _historyService.GetSummary();
            return Ok(result);
        }
    }
}
