using AI_as_a_Service.Data;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Services.Interfaces;
using Newtonsoft.Json;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FineTuningController : ControllerBase
    {
        private readonly ILogger<FineTuningController> _logger;
        private readonly IFineTuningService _fineTuningService;

        public FineTuningController(ILogger<FineTuningController> logger, IFineTuningService fineTuningService)
        {
            _logger = logger;
            _fineTuningService = fineTuningService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFineTune([FromBody] CreateFineTuneRequest request)
        {
            _logger.LogInformation("Create FineTune");

            try
            {
                var fineTuning = await _fineTuningService.CreateFineTuneAsync(request);
                return Ok(fineTuning);
            }
            catch (Exception ex)
            {
                // Handle the error as appropriate for your application
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListFineTunes()
        {
            _logger.LogInformation("List all FineTunes");

            var fineTunes = await _fineTuningService.ListFineTunesAsync();
            return Ok(fineTunes);
        }

        [HttpGet("{fineTuneId}")]
        public async Task<IActionResult> RetrieveFineTune(int fineTuneId)
        {
            _logger.LogInformation("Get FineTune by ID");

            var fineTuning = await _fineTuningService.RetrieveFineTuneAsync(fineTuneId);

            if (fineTuning == null)
            {
                return NotFound();
            }

            return Ok(fineTuning);
        }

        [HttpPost("{fineTuneId}/cancel")]
        public async Task<IActionResult> CancelFineTune(int fineTuneId)
        {
            _logger.LogInformation("Cancel FineTune");

            try
            {
                await _fineTuningService.CancelFineTuneAsync(fineTuneId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle the error as appropriate for your application
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}