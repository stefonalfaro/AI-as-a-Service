using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using Plan = AI_as_a_Service.Models.Plan;

namespace AI_as_a_Service.Services
{
    public class PlanService : IPlanService
    {
        private readonly IRepository<Plan> _dataAccessLayer;
        private readonly Configuration _configuration;
        private readonly IHubContext<ChatHub> _stateManagement;
        private readonly ILogger<PlanService> _logger;

        public PlanService(IRepository<Plan> dataAccessLayer, IHubContext<ChatHub> stateManagement, Configuration configuration)
        {
            _dataAccessLayer = dataAccessLayer;
            _stateManagement = stateManagement;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Models.Plan>> GetPlansAsync()
        {
            return await _dataAccessLayer.GetAllAsync();
        }

        public async Task<Plan> GetPlanAsync(Guid id)
        {
            return await _dataAccessLayer.GetByIdAsync(id);
        }

        public async Task<Plan> CreatePlanAsync(Plan plan)
        {
            await _dataAccessLayer.AddAsync(plan);
            return plan;
        }

        public async Task UpdatePlanAsync(int id, Plan plan)
        {
            if (id != plan.id)
            {
                throw new ArgumentException("Invalid plan ID");
            }

            await _dataAccessLayer.UpdateAsync(plan);

            // Send the updated plan to the SignalR clients
            await _stateManagement.Clients.All.SendAsync("PlanUpdated", plan);
        }

        public async Task DeletePlanAsync(int id)
        {
            await _dataAccessLayer.DeleteAsync(id);

            // Send the deleted plan to the SignalR clients
            await _stateManagement.Clients.All.SendAsync("PlanDeleted", id);
        }

        Task<Plan> IPlanService.GetPlanAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Models.Plan>> IPlanService.GetPlansAsync()
        {
            throw new NotImplementedException();
        }

        Task<Models.Plan> IPlanService.GetPlanAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Models.Plan> IPlanService.CreatePlanAsync(Models.Plan plan)
        {
            throw new NotImplementedException();
        }

        Task IPlanService.UpdatePlanAsync(int id, Models.Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}