using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {

        public SummaryController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetSummary([FromBody] string input)
        {
            //Handoff to histroy service to get summary
            return Ok();
        }

    }
}