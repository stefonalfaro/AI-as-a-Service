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

        public ChatCompletionController(OpenAI openAI)
        {
            _openAI = openAI;
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletion([FromBody] ChatCompletionRequest request)
        {
            var messages = request.Messages.Select(m => new ChatMessage
            {
                Role = m.Role,
                Content = m.Content
            }).ToList();

            var response = await _openAI.ChatCompletionAsync(messages);

            return Ok(new { response });
        }
    }
}
