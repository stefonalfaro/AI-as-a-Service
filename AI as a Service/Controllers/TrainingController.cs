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
using Accord.MachineLearning;
using Accord.Statistics.Models.Regression;

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
            // Train the model using linear regression
            var trainedModel = _trainingService.TrainLinearRegression();

            // Save the model or its parameters for later use
            _trainingService.SaveTrainedModel(trainedModel);

            // Return the result
            return Ok(new { Model = trainedModel });
        }

        [HttpGet("predict")]
        public IActionResult Predict([FromQuery] double[] input)
        {
            // Load the trained model
            var trainedModel = _trainingService.LoadTrainedModel();

            if (trainedModel == null)
            {
                return BadRequest("Model not found or not trained yet.");
            }

            // Make a prediction
            double prediction = _trainingService.Predict(trainedModel, input);

            return Ok(new { Prediction = prediction });
        }

        [HttpPost("train-kmeans-clustering")]
        public IActionResult TrainKMeansClustering(double[][] inputs, int numberOfClusters)
        {
            KMeans kmeans = _trainingService.TrainKMeansClustering(inputs, numberOfClusters);
            return Ok(new { ClusteringModel = kmeans });
        }

        [HttpPost("train-logistic-regression")]
        public IActionResult TrainLogisticRegression([FromBody] LogisticRegressionTrainingData trainingData)
        {
            LogisticRegression logisticRegression = _trainingService.TrainLogisticRegression(trainingData.Inputs, trainingData.Outputs);
            return Ok(new { RegressionModel = logisticRegression });
        }

        [HttpPost("train-dbscan")]
        public IActionResult TrainDBSCAN(double[][] inputs, double epsilon, int minPoints)
        {

            // Train the DBSCAN clustering algorithm
            //DBSCAN dbscan = _trainingService.TrainDBSCAN(inputs, epsilon, minPoints);

            // Return the result
            //return Ok(new { Model = dbscan });
            return Ok();
        }

        // Similarly, create functions for other algorithms
        [HttpPost("train-gaussian-mixture-model")]
        public IActionResult TrainGaussianMixtureModel(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-grubbs-test")]
        public IActionResult TrainGrubbsTest(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-autoencoder")]
        public IActionResult TrainAutoencoder(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-decision-trees")]
        public IActionResult TrainDecisionTrees(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-random-forests")]
        public IActionResult TrainRandomForests(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-gradient-boosted-trees")]
        public IActionResult TrainGradientBoostedTrees(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-support-vector-machines")]
        public IActionResult TrainSupportVectorMachines(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-feedforward-neural-network")]
        public IActionResult TrainFeedforwardNeuralNetwork(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-convolutional-neural-network")]
        public IActionResult TrainConvolutionalNeuralNetwork(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-recurrent-neural-network")]
        public IActionResult TrainRecurrentNeuralNetwork(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-k-nearest-neighbors")]
        public IActionResult TrainKNearestNeighbors(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-time-series-models")]
        public IActionResult TrainTimeSeriesModels(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-support-vector-regression")]
        public IActionResult TrainSupportVectorRegression(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-random-forest-regression")]
        public IActionResult TrainRandomForestRegression(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }

        [HttpPost("train-gradient-boosting-regression")]
        public IActionResult TrainGradientBoostingRegression(/* your input parameters */)
        {
            // ...
            return new OkResult();
        }
    }
}