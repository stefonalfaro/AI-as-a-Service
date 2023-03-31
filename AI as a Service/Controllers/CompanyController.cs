using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AI_as_a_Service.Data;
using Microsoft.AspNetCore.Authorization;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly Configuration _configuration;
        private readonly ILogger<CompanyController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Company> _dataAccessLayer;

        public CompanyController(ICompanyService companyService, Configuration configuration, ILogger<CompanyController> logger, IHubContext<ChatHub> hubContext, IRepository<Company> dataAccessLayer)
        {
            _companyService = companyService;
            _configuration = configuration;
            _logger = logger;
            _hubContext = hubContext;
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpGet]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            _logger.LogInformation("Get all Companies");

            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        // Add other actions such as GetById, Create, Update, Delete as needed
    }
}
