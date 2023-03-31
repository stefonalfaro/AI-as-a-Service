using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IConfigurationService
    {
        Task<Configuration> GetConfigurationAsync();
        Task UpdateConfigurationAsync(Configuration newConfiguration);
    }
}
