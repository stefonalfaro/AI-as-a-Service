﻿namespace AI_as_a_Service.Models
{
    [Serializable]
    public class TrainedModel
    {
        public double[] Weights { get; set; }
        public double Intercept { get; set; }
    }

    public class LogisticRegressionTrainingData
    {
        public double[][] Inputs { get; set; }
        public int[] Outputs { get; set; }
    }
}
