using Microsoft.AspNetCore.Mvc;
using AI_as_a_Service.Models;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Middlewares;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using static AI_as_a_Service.Models.ChatCompletions;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatCompletionController : ControllerBase
    {
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ILogger<ChatCompletionController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatCompletionController(IChatCompletionService chatCompletionService, ILogger<ChatCompletionController> logger, IHubContext<ChatHub> hubContext)
        {
            _chatCompletionService = chatCompletionService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletion([FromBody] ChatCompletionRequest request)
        {
            _logger.LogInformation("Get Chat Completion");

            var response = await _chatCompletionService.GetCompletionAsync(request);
            return Ok(new { response });
        }
    }

}