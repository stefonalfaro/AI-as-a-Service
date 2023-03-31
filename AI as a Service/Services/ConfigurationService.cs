using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AI_as_a_Service.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IRepository<Configuration> _dataAccessLayer;
        private readonly IHubContext<ChatHub> _stateManagement;

        public ConfigurationService(IRepository<Configuration> dataAccessLayer, IHubContext<ChatHub> stateManagement)
        {
            _dataAccessLayer = dataAccessLayer;
            _stateManagement = stateManagement;
        }

        public async Task<Configuration> GetConfigurationAsync()
        {
            return await _dataAccessLayer.GetByIdAsync(0);
        }

        public async Task UpdateConfigurationAsync(Configuration newConfiguration)
        {
            await _dataAccessLayer.UpdateAsync(newConfiguration);
            await _stateManagement.Clients.All.SendAsync("ConfigurationUpdated", newConfiguration);
        }
    }

}
