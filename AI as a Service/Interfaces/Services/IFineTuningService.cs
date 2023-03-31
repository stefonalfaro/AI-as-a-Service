using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IFineTuningService
    {
        Task<FineTuning> CreateFineTuneAsync(CreateFineTuneRequest request);
        Task<IEnumerable<FineTuning>> ListFineTunesAsync();
        Task<FineTuning> RetrieveFineTuneAsync(int fineTuneId);
        Task CancelFineTuneAsync(int fineTuneId);
        Task<FineTuning> RetrieveFineTuneAsync(Guid fineTuneId);
        Task CancelFineTuneAsync(Guid fineTuneId);
    }
}