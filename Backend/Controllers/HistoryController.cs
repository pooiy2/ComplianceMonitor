using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ease.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _historyService.GetHistory();
            return Ok(result);
        }
    }
}
