using Microsoft.AspNetCore.Mvc;
using AI_as_a_Service.Models;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Middlewares;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using static AI_as_a_Service.Models.ChatCompletions;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatCompletionController : ControllerBase
    {
        private readonly OpenAISDK _openAI;
        private readonly Configuration _configuration;
        private readonly ILogger<ChatCompletionController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<ChatCompletions> _dataAccessLayer;

        public ChatCompletionController(Configuration configuration, ILogger<ChatCompletionController> logger, IHubContext<ChatHub> hubContext, IRepository<ChatCompletions> dataAccessLayer)
        {
            _openAI = new OpenAISDK(configuration.integrationSettings.OpenAPIKey);
            _configuration = configuration;
            _logger = logger;
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletion([FromBody] ChatCompletionRequest request)
        {
            var response = "";

            return Ok(new { response });
        }
    }
}