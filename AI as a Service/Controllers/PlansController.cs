using AI_as_a_Service.Data;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly ILogger<PlansController> _logger;
        private readonly IPlanService _planService;
        private readonly IHubContext<ChatHub> _stateManagement;

        public PlansController(ILogger<PlansController> logger, IPlanService planService, IHubContext<ChatHub> stateManagement)
        {
            _logger = logger;
            _planService = planService;
            _stateManagement = stateManagement;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            var plans = await _planService.GetPlansAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(Guid id)
        {
            var plan = await _planService.GetPlanAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<Plan>> CreatePlan(Plan plan)
        {
            var createdPlan = await _planService.CreatePlanAsync(plan);
            return CreatedAtAction(nameof(GetPlan), new { id = createdPlan.id }, createdPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(int id, Plan plan)
        {
            try
            {
                await _planService.UpdatePlanAsync(id, plan);

                // Send the updated plan to the SignalR clients
                await _stateManagement.Clients.All.SendAsync("PlanUpdated", plan);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                var existingPlan = await _planService.GetPlanAsync(id);

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
            var plan = await _planService.GetPlanAsync(id);

            if (plan == null)
            {
                return NotFound();
            }
            await _planService.DeletePlanAsync(id);
            return NoContent();
        }
    }
}