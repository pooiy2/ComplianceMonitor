using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {

        public HistoryController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetHistory([FromBody] string input)
        {
            //Handoff to service layer to get history
            return Ok();
        }

    }
}