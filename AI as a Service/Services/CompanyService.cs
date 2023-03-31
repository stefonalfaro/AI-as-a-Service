using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AI_as_a_Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _dataAccessLayer;
        private readonly IHubContext<ChatHub> _stateManagement;

        public CompanyService(IRepository<Company> dataAccessLayer, IHubContext<ChatHub> stateManagement)
        {
            _dataAccessLayer = dataAccessLayer;
            _stateManagement = stateManagement;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _dataAccessLayer.GetAllAsync();
        }

        public async Task<Company> GetCompanyAsync(int id)
        {
            return await _dataAccessLayer.GetByIdAsync(id);
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            await _dataAccessLayer.AddAsync(company);
            await _stateManagement.Clients.All.SendAsync("CompanyCreated", company);
            return company;
        }

        public async Task UpdateCompanyAsync(int id, Company company)
        {
            if (id != company.id)
            {
                throw new ArgumentException("Invalid company ID");
            }

            await _dataAccessLayer.UpdateAsync(company);
            await _stateManagement.Clients.All.SendAsync("CompanyUpdated", company);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _dataAccessLayer.GetByIdAsync(id);
            if (company != null)
            {
                await _dataAccessLayer.DeleteAsync(id);
                await _stateManagement.Clients.All.SendAsync("CompanyDeleted", company);
            }
        }
    }

}
