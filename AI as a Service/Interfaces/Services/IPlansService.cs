using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<Plan>> GetPlansAsync();
        Task<Plan> GetPlanAsync(Guid id);
        Task<Plan> GetPlanAsync(int id);
        Task<Plan> CreatePlanAsync(Plan plan);
        Task UpdatePlanAsync(int id, Plan plan);
        Task DeletePlanAsync(int id);
    }
}