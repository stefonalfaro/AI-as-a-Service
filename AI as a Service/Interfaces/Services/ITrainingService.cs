using AI_as_a_Service.Models;
using System.Collections.Generic;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface ITrainingService
    {
        TrainedModel TrainLinearRegression(double[][] inputs, double[] outputs);
        void SaveTrainedModel(TrainedModel model);
        TrainedModel LoadTrainedModel();
        double Predict(TrainedModel model, double[] input);
    }
}
