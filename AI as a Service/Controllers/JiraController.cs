using AI_as_a_Service.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraController : ControllerBase
    {
        private readonly JiraApiClient _jiraApiClient;

        public JiraController(JiraApiClient jiraApiClient)
        {
            _jiraApiClient = jiraApiClient;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchIssues([FromQuery] string jql, [FromQuery] int startAt = 0, [FromQuery] int maxResults = 50)
        {
            try
            {
                var result = await _jiraApiClient.SearchIssuesAsync(jql, startAt, maxResults);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
