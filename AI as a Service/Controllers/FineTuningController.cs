using AI_as_a_Service.Helpers;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FineTuningController : ControllerBase
    {
        private readonly OpenAI _openAI;
        private readonly Configuration _configuration;
        private readonly ILogger<FineTuningController> _logger;
        public FineTuningController(Configuration configuration, ILogger<FineTuningController> logger)
        {
            _openAI = new OpenAI(configuration.integrationSettings.OpenAPIKey);
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFineTune([FromBody] CreateFineTuneRequest request)
        {
            _logger.LogInformation("Create FineTune");

            var parameters = new FineTuningParameters
            {
                ValidationFileId = request.ValidationFile,
                //Model = request.Model,
                //NumberOfEpochs = request.NEpochs,
                //BatchSize = request.BatchSize,
                //LearningRateMultiplier = request.LearningRateMultiplier,
                //PromptLossWeight = request.PromptLossWeight,
                //ComputeClassificationMetrics = request.ComputeClassificationMetrics,
                //ClassificationNumberOfClasses = request.ClassificationNClasses,
                //ClassificationPositiveClass = request.ClassificationPositiveClass,
                //ClassificationBetas = request.ClassificationBetas,
                //Suffix = request.Suffix
            };

            var response = await _openAI.CreateFineTuneAsync(request.TrainingFile, parameters);

            if (response.IsSuccessStatusCode)
            {
                var fineTune = await response.Content.ReadAsStringAsync();
                return Ok(fineTune);
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListFineTunes()
        {
            _logger.LogInformation("List all FuneTunes");

            var fineTunes = await _openAI.ListFineTunesAsync();
            return Ok(fineTunes);
        }

        [HttpGet("{fineTuneId}")]
        public async Task<IActionResult> RetrieveFineTune(string fineTuneId)
        {
            _logger.LogInformation("Get FineTune by ID");

            var fineTune = await _openAI.RetrieveFineTuneAsync(fineTuneId);
            return Ok(fineTune);
        }

        [HttpPost("{fineTuneId}/cancel")]
        public async Task<IActionResult> CancelFineTune(string fineTuneId)
        {
            _logger.LogInformation("Cancel FineTune");

            await _openAI.CancelFineTuneAsync(fineTuneId);
            return Ok();
        }
    }
}