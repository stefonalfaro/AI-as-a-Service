using Newtonsoft.Json;

namespace AI_as_a_Service.Models
{
    public class FineTuning
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public List<QuestionResponsePair> QuestionResponsePairs { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModified { get; set; }
        public int createdByUserId { get; set; }
        public int lastModifiedByUserId { get; set; }
    }

    public class QuestionResponsePair
    {
        public string Question { get; set; }
        public string Response { get; set; }
    }

    public class FineTuningParameters
    {
        public string ValidationFileId { get; set; }
        // Add other properties for the optional parameters
    }

    public class FineTunesListResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public List<FineTune> Data { get; set; }
    }

    public class FineTune
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        // Other properties for the fine-tune object
    }

    public class CreateFineTuneRequest
    {
        public string TrainingFile { get; set; }
        public string ValidationFile { get; set; }
        public string Model { get; set; }
        public int? NEpochs { get; set; }
        public int? BatchSize { get; set; }
        public double? LearningRateMultiplier { get; set; }
        public double? PromptLossWeight { get; set; }
        public bool? ComputeClassificationMetrics { get; set; }
        public int? ClassificationNClasses { get; set; }
        public string ClassificationPositiveClass { get; set; }
        public double[] ClassificationBetas { get; set; }
        public string Suffix { get; set; }
    }

}
