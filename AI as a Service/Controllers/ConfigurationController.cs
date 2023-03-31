using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly Configuration _configuration;
        public ConfigurationController(Configuration configuration, ILogger<ConfigurationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<Configuration> GetConfiguration()
        {
            _logger.LogInformation("Get Configuration");

            return Ok(_configuration);
        }

        [HttpPut]
        public ActionResult UpdateConfiguration([FromBody] Configuration newConfiguration)
        {
            _logger.LogInformation("Update Configuration");

            _configuration.FreemiumTimer = newConfiguration.FreemiumTimer;
            _configuration.DisableAllLogin = newConfiguration.DisableAllLogin;
            return NoContent();
        }
    }
}
