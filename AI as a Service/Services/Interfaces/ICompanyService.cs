using AI_as_a_Service.Models;

namespace AI_as_a_Service.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        // Add other methods such as GetById, Create, Update, Delete as needed
    }
}
