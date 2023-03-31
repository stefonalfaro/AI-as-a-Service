using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyAsync(int id);
        Task<Company> CreateCompanyAsync(Company company);
        Task UpdateCompanyAsync(int id, Company company);
        Task DeleteCompanyAsync(int id);
    }
}