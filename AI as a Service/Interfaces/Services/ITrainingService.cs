using Accord.MachineLearning;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Testing;
using AI_as_a_Service.Models;
using System.Collections.Generic;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface ITrainingService
    {
        TrainedModel TrainLinearRegression();
        void SaveTrainedModel(TrainedModel model);
        TrainedModel LoadTrainedModel();
        double Predict(TrainedModel model, double[] input);

        KMeans TrainKMeansClustering(double[][] inputs, int numberOfClusters);
        //DBSCAN<double[]> TrainDBSCAN(double[][] inputs, double radius, int minPoints);
        GaussianMixtureModel TrainGaussianMixtureModel(double[][] inputs, int numberOfComponents);
        GrubbTest TrainGrubbsTest(double[] input);

        TrainedModel TrainAutoencoder(/* Add required parameters */);
        LogisticRegression TrainLogisticRegression(double[][] inputs, int[] outputs);

        TrainedModel TrainDecisionTrees(double[][] inputs, int[] outputs);
        TrainedModel TrainRandomForests(double[][] inputs, int[] outputs);
        TrainedModel TrainGradientBoostedTrees(double[][] inputs, int[] outputs);

        TrainedModel TrainSupportVectorMachines(double[][] inputs, int[] outputs);
        TrainedModel TrainFeedforwardNeuralNetwork(double[][] inputs, int[] outputs);
        TrainedModel TrainConvolutionalNeuralNetwork(double[][] inputs, int[] outputs);
        TrainedModel TrainRecurrentNeuralNetwork(double[][] inputs, int[] outputs);

        TrainedModel TrainKNearestNeighbors(double[][] inputs, int[] outputs, int k);

        TrainedModel TrainTimeSeriesModels(double[] input, string modelType);
        TrainedModel TrainSupportVectorRegression(double[][] inputs, double[] outputs);
        TrainedModel TrainRandomForestRegression(double[][] inputs, double[] outputs);
        TrainedModel TrainGradientBoostingRegression(double[][] inputs, double[] outputs);
    }
}
