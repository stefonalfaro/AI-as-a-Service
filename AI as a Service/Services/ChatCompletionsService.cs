using AI_as_a_Service.Helpers;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using static AI_as_a_Service.Models.ChatCompletions;

namespace AI_as_a_Service.Services
{
    public class ChatCompletionService : IChatCompletionService
    {
        private readonly OpenAISDK _openAI;
        private readonly Configuration _configuration;
        private readonly IRepository<ChatCompletions> _dataAccessLayer;

        public ChatCompletionService(Configuration configuration, IRepository<ChatCompletions> dataAccessLayer)
        {
            _openAI = new OpenAISDK(configuration.integrationSettings.OpenAPIKey);
            _configuration = configuration;
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<string> GetCompletionAsync(ChatCompletionRequest request)
        {
            // Add your logic to get completion here
            var response = "";

            return response;
        }
    }

}
