using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Configuration _configuration;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService userService, Configuration configuration, ILogger<UsersController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            _logger.LogInformation("Get all Users");

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Add other actions such as GetById, Create, Update, Delete as needed
    }
}
