using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly Configuration _configuration;
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(Configuration configuration, ILogger<ConfigurationController> logger, IConfigurationService configurationService)
        {
            _configuration = configuration;
            _logger = logger;
            _configurationService = configurationService;
        }

        // Get configuration
        [HttpGet]
        public async Task<ActionResult<Configuration>> GetConfiguration()
        {
            _logger.LogInformation("Get Configuration");

            var configuration = await _configurationService.GetConfigurationAsync();
            return Ok(configuration);
        }

        // Update configuration
        [HttpPut]
        public async Task<ActionResult> UpdateConfiguration([FromBody] Configuration newConfiguration)
        {
            _logger.LogInformation("Update Configuration");

            try
            {
                await _configurationService.UpdateConfigurationAsync(newConfiguration);

                // Update the in-memory configuration
                _configuration.FreemiumTimer = newConfiguration.FreemiumTimer;
                _configuration.DisableAllLogin = newConfiguration.DisableAllLogin;

                return NoContent();
            }
            catch (InvalidOperationException)
            {
                var existingConfiguration = await _configurationService.GetConfigurationAsync();

                if (existingConfiguration == null)
                {
                    return NotFound();
                }

                throw;
            }
        }
    }

}
