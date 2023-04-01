using Accord.Math.Optimization.Losses;
using Accord.Statistics.Models.Regression.Linear;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace AI_as_a_Service.Controllers
{
    [Authorize(Roles = "MasterAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly ILogger<TrainingController> _logger;

        public TrainingController(ITrainingService trainingService, ILogger<TrainingController> logger)
        {
            _trainingService = trainingService;
            _logger = logger;
        }

        [HttpPost("train-linear-regression")]
        public IActionResult TrainLinearRegression()
        {
            // Prepare your training data
            // Replace with your actual data and preprocessing steps
            double[][] inputs = new double[][] {
                new double[] { 1.0, 2.0, 3.0 },
                new double[] { 4.0, 5.0, 6.0 },
                new double[] { 7.0, 8.0, 9.0 },
                new double[] { 10.0, 11.0, 12.0 }}; 
            
            double[] outputs = new double[] { 2.0, 5.0, 8.0, 11.0 };

            // Train the model using linear regression
            var trainedModel = _trainingService.TrainLinearRegression(inputs, outputs);

            // Evaluate the model
            //double[] predictedOutputs = trainedModel.Predict(inputs);
            var loss = new SquareLoss(outputs);
            //double error = loss.Loss(predictedOutputs);

            // Save the model or its parameters for later use
            _trainingService.SaveTrainedModel(trainedModel);

            // Update clients using SignalR


            // Return the result
            return Ok(new { Model = trainedModel, Error = "" });
        }

        [HttpGet("predict")]
        public IActionResult Predict(/* your input parameters */)
        {
            // Load the trained model
            var trainedModel = _trainingService.LoadTrainedModel();

            if (trainedModel == null)
            {
                return BadRequest("Model not found or not trained yet.");
            }

            // Prepare the input for prediction
            double[] input = new double[] { 1.0, 2.0, 3.0 };

            // Make a prediction
            double prediction = _trainingService.Predict(trainedModel, input);

            return Ok(new { Prediction = prediction });
        }
    }
}