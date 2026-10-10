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
        public async Task<IActionResult> PostAnalyze(HuggingFaceRequestViewModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Action) || string.IsNullOrWhiteSpace(request.Guideline))
            {
                return BadRequest("Invalid request. Please provide both 'Input' and 'Guideline'.");
            }

            return Ok(await _huggingFaceService.AnalyzeTextAsync(request));
        }

    }
}