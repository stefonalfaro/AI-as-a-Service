using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using AI_as_a_Service.Services.Interfaces;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly Configuration _configuration;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Configuration> _dataAccessLayer;

        public ConfigurationController(Configuration configuration, ILogger<ConfigurationController> logger, IHubContext<ChatHub> hubContext, IRepository<Configuration> dataAccessLayer)
        {
            _configuration = configuration;
            _logger = logger;
            _hubContext = hubContext;
            _dataAccessLayer = dataAccessLayer;
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
