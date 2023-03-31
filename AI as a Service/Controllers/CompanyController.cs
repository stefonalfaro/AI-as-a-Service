using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly Configuration _configuration;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, Configuration configuration, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            _logger.LogInformation("Get all Companies");

            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "MasterAdmin,CompanyAdmin,User")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            _logger.LogInformation("Get Company by ID");

            var company = await _companyService.GetCompanyAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPost]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            _logger.LogInformation("Create Company");

            var createdCompany = await _companyService.CreateCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.id }, createdCompany);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MasterAdmin,CompanyAdmin")]
        public async Task<IActionResult> UpdateCompany(int id, Company company)
        {
            _logger.LogInformation("Update Company");

            try
            {
                await _companyService.UpdateCompanyAsync(id, company);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                var existingCompany = await _companyService.GetCompanyAsync(id);

                if (existingCompany == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            _logger.LogInformation("Delete Company");

            var company = await _companyService.GetCompanyAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }
    }
}
