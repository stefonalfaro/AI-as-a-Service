using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            _logger.LogInformation("Get all Companies");

            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        // Add other actions such as GetById, Create, Update, Delete as needed
    }
}
