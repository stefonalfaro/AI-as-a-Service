using AI_as_a_Service.Helpers;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace AI_as_a_Service.Services
{
    public class FineTuningService : IFineTuningService
    {
        private readonly OpenAISDK _openAI;
        private readonly IRepository<FineTuning> _dataAccessLayer;
        private readonly IHubContext<ChatHub> _stateManagement;

        public FineTuningService(OpenAISDK openAI, IRepository<FineTuning> dataAccessLayer, IHubContext<ChatHub> stateManagement)
        {
            _openAI = openAI;
            _dataAccessLayer = dataAccessLayer;
            _stateManagement = stateManagement;
        }

        public async Task<FineTuning> CreateFineTuneAsync(string trainingFile, CreateFineTuneRequest request)
        {
            var parameters = new FineTuningParameters
            {
                ValidationFileId = request.ValidationFile,
                // ... (other parameters)
            };

            var response = await _openAI.CreateFineTuneAsync(trainingFile, parameters);

            if (response.IsSuccessStatusCode)
            {
                var fineTune = await response.Content.ReadAsStringAsync();

                // Save the fine-tune result in the database using the Data Access Layer
                var fineTuning = JsonConvert.DeserializeObject<FineTuning>(fineTune);
                await _dataAccessLayer.AddAsync(fineTuning);

                // Send the fine-tune result to the front-end using the State Management layer
                await _stateManagement.Clients.All.SendAsync("FineTuneCreated", fineTuning);

                return fineTuning;
            }
            else
            {
                // Handle the error as appropriate for your application
                throw new Exception("Error creating fine-tune.");
            }
        }

        public async Task<IEnumerable<FineTuning>> ListFineTunesAsync()
        {
            return await _dataAccessLayer.GetAllAsync();
        }

        public async Task<FineTuning> RetrieveFineTuneAsync(int fineTuneId)
        {
            return await _dataAccessLayer.GetByIdAsync(fineTuneId);
        }

        public async Task CancelFineTuneAsync(int fineTuneId)
        {
            await _openAI.CancelFineTuneAsync(fineTuneId.ToString());

            // Delete the fine-tune from the database using the Data Access Layer
            await _dataAccessLayer.DeleteAsync(fineTuneId);

            // Send the deleted fine-tune ID to the front-end using the State Management layer
            await _stateManagement.Clients.All.SendAsync("FineTuneCanceled", fineTuneId);
        }

        Task<FineTuning> IFineTuningService.CreateFineTuneAsync(CreateFineTuneRequest request)
        {
            throw new NotImplementedException();
        }

        Task<FineTuning> IFineTuningService.RetrieveFineTuneAsync(Guid fineTuneId)
        {
            throw new NotImplementedException();
        }

        Task IFineTuningService.CancelFineTuneAsync(Guid fineTuneId)
        {
            throw new NotImplementedException();
        }
    }
}