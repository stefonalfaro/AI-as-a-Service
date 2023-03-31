using AI_as_a_Service.Data;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AI_as_a_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            _logger.LogInformation("Get all Users");

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            _logger.LogInformation("Get User by ID");

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _logger.LogInformation("Create User");

            var createdUser = await _userService.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetUserById), new
            {
                id = createdUser.id
            }, createdUser);
        }

        // Add other actions such as Update, Delete as needed

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount(Guid id, string verificationCode)
        {
            _logger.LogInformation("Activate Account");

            var result = await _userService.ActivateAccountAsync(id, verificationCode);

            if (result)
            {
                return Ok();
            }

            return BadRequest("Invalid verification code.");
        }
    }
}
