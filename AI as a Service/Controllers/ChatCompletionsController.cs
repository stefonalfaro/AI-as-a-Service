using Microsoft.AspNetCore.Mvc;
using AI_as_a_Service.Models;
using AI_as_a_Service.Helpers;
using static AI_as_a_Service.Models.ChatCompletions;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatCompletionController : ControllerBase
    {
        private readonly OpenAI _openAI;
        private readonly Configuration _configuration;
        private readonly ILogger<ChatCompletionController> _logger;
        public ChatCompletionController(OpenAI openAI, Configuration configuration, ILogger<ChatCompletionController> logger)
        {
            _openAI = new OpenAI(configuration.integrationSettings.OpenAPIKey);
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletion([FromBody] ChatCompletionRequest request)
        {
            var response = "";

            return Ok(new { response });
        }
    }
}