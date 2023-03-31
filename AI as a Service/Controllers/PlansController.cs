using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly ILogger<PlansController> _logger;
        private readonly Configuration _configuration;
        public PlansController(ILogger<PlansController> logger, Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var plans = new List<Plan>
        {
            new Plan { id = 1, price = 9.99m, term = "Monthly", name = "Basic", description = "Basic plan with limited features" },
            new Plan { id = 2, price = 29.99m, term = "Monthly", name = "Pro", description = "Pro plan with more features" },
            new Plan { id = 3, price = 99.99m, term = "Monthly", name = "Enterprise", description = "Enterprise plan with all features" },
        };

            _logger.LogInformation("Retrieved plans");
            return Ok(plans);
        }
    }
}
