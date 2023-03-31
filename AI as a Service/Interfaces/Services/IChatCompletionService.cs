using static AI_as_a_Service.Models.ChatCompletions;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IChatCompletionService
    {
        Task<string> GetCompletionAsync(ChatCompletionRequest request);
    }
}