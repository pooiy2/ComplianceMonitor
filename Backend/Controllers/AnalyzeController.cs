using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyzeController : ControllerBase
    {

        public AnalyzeController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> Analyze([FromBody] string input)
        {
            //Handoff to service layer to call hugging face
            return Ok();
        }

    }
}