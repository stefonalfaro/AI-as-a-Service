using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;

namespace AI_as_a_Service.Services
{
    public class PlanService : IPlanService
    {
        private readonly IRepository<Plan> _dataAccessLayer;

        public PlanService(IRepository<Plan> dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<Plan>> GetPlansAsync()
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
        }

        public async Task DeletePlanAsync(int id)
        {
            await _dataAccessLayer.DeleteAsync(id);
        }

        Task<Plan> IPlanService.GetPlanAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
