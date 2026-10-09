using Backend.Services;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyzeController : ControllerBase
    {
        private readonly IHuggingFaceService _huggingFaceService;

        public AnalyzeController(IHuggingFaceService huggingFaceService)
        {
            _huggingFaceService = huggingFaceService;
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(HuggingFaceRequestViewModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Input) || string.IsNullOrWhiteSpace(request.Guideline))
            {
                return BadRequest("Invalid request. Please provide both 'Input' and 'Guideline'.");
            }

            //Handoff to service layer to call hugging face
            return Ok(await _huggingFaceService.AnalyzeTextAsync(request));
        }

    }
}