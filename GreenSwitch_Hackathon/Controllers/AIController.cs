using Microsoft.AspNetCore.Mvc;
using GreenSwitch.Models;
using GreenSwitch.Services;

namespace GreenSwitch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly ISmartAIService _aiService;

        public AIController(ISmartAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponse>> Chat([FromBody] ChatRequest request)
        {
            try
            {
                var response = await _aiService.ProcessMessageAsync(request.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ChatResponse
                {
                    Success = false,
                    Response = "I'm having trouble accessing our supplier database. Please try again later."
                });
            }
        }
    }
}