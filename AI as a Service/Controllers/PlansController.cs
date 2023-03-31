using AI_as_a_Service.Data;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using AI_as_a_Service.Services.Interfaces;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly ILogger<PlansController> _logger;
        private readonly Configuration _configuration;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Plan> _dataAccessLayer;
        public PlansController(ILogger<PlansController> logger, Configuration configuration, IHubContext<ChatHub> hubContext, IRepository<Plan> dataAccessLayer)
        {
            _logger = logger;
            _configuration = configuration;
            _hubContext = hubContext;
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            var plans = await _dataAccessLayer.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(Guid id)
        {
            var plan = await _dataAccessLayer.GetByIdAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<Plan>> CreatePlan(Plan plan)
        {
            await _dataAccessLayer.AddAsync(plan);
            return CreatedAtAction(nameof(GetPlan), new { id = plan.id }, plan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(int id, Plan plan)
        {
            if (id != plan.id)
            {
                return BadRequest();
            }

            try
            {
                await _dataAccessLayer.UpdateAsync(plan);
            }
            catch (InvalidOperationException)
            {
                var existingPlan = await _dataAccessLayer.GetByIdAsync(id);

                if (existingPlan == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var plan = await _dataAccessLayer.GetByIdAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            await _dataAccessLayer.DeleteAsync(id);
            return NoContent();
        }
    }
}