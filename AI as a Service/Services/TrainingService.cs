using Accord.Statistics.Models.Regression.Linear;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Controllers;

namespace AI_as_a_Service.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly Configuration _configuration;
        private readonly IRepository<User> _dataAccessLayer;
        private readonly IHubContext<ChatHub> _stateManagement;
        private readonly ILogger<TrainingService> _logger;

        public TrainingService(Configuration configuration, IRepository<User> dataAccessLayer, IHubContext<ChatHub> stateManagement, ILogger<TrainingService> logger)
        {
            _configuration = configuration;
            _dataAccessLayer = dataAccessLayer;
            _stateManagement = stateManagement;
            _logger = logger;
        }

        public TrainedModel TrainLinearRegression(double[][] inputs, double[] outputs)
        {
            var ols = new OrdinaryLeastSquares();
            var model = ols.Learn(inputs, outputs);
            return new TrainedModel { Weights = model.Weights, Intercept = model.Intercept };
        }

        public void SaveTrainedModel(TrainedModel model)
        {
            using (FileStream stream = new FileStream(ModelFilePath, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, model);
            }
        }

        public TrainedModel LoadTrainedModel()
        {
            if (!File.Exists(ModelFilePath))
            {
                return null;
            }

            using (FileStream stream = new FileStream(ModelFilePath, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                return (TrainedModel)formatter.Deserialize(stream);
            }
        }

        public double Predict(TrainedModel trainedModel, double[] input)
        {
            var model = new MultipleLinearRegression(1);
            return model.Transform(input);
        }

        private string ModelFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trained_model.bin");
    }
}
