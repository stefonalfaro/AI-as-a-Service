using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AI_as_a_Service.Models;
using Newtonsoft.Json;

namespace AI_as_a_Service.Helpers
{
    public class OpenAISDK
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAISDK(string apiKey)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.openai.com/v1/") };
            _apiKey = apiKey;
        }

        public async Task<string> ChatCompletionAsync(List<ChatCompletions.Message> messages)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var model = "gpt-3.5-turbo";

            var requestContent = new
            {
                model,
                messages
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("davinci-codex/completions", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonDocument.Parse(responseContent);

            return responseObject.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }

        public async Task<HttpResponseMessage> CreateFineTuneAsync(string trainingFileId, FineTuningParameters parameters)
        {
            var requestBody = new
            {
                training_file = trainingFileId,
                validation_file = parameters.ValidationFileId,
                //model = parameters.Model,
                //n_epochs = parameters.NumberOfEpochs,
                //batch_size = parameters.BatchSize,
                //learning_rate_multiplier = parameters.LearningRateMultiplier,
                //prompt_loss_weight = parameters.PromptLossWeight,
                //compute_classification_metrics = parameters.ComputeClassificationMetrics,
                //classification_n_classes = parameters.ClassificationNumberOfClasses,
                //classification_positive_class = parameters.ClassificationPositiveClass,
                //classification_betas = parameters.ClassificationBetas,
                //suffix = parameters.Suffix
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/fine-tunes", content);
            return response;
        }

        public async Task<List<FineTune>> ListFineTunesAsync()
        {
            var response = await _httpClient.GetAsync("https://api.openai.com/v1/fine-tunes");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var fineTunesList = JsonConvert.DeserializeObject<FineTunesListResponse>(jsonResponse);
            return fineTunesList.Data;
        }

        public async Task<FineTune> RetrieveFineTuneAsync(string fineTuneId)
        {
            var response = await _httpClient.GetAsync($"https://api.openai.com/v1/fine-tunes/{fineTuneId}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var fineTune = JsonConvert.DeserializeObject<FineTune>(jsonResponse);
            return fineTune;
        }

        public async Task CancelFineTuneAsync(string fineTuneId)
        {
            var response = await _httpClient.PostAsync($"https://api.openai.com/v1/fine-tunes/{fineTuneId}/cancel", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
