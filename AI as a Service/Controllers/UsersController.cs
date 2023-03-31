using AI_as_a_Service.Data;
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
        private readonly Configuration _configuration;
        private readonly ILogger<UsersController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<User> _dataAccessLayer;
        private readonly EmailService _emailService;

        public UsersController(IUserService userService, Configuration configuration, ILogger<UsersController> logger, IHubContext<ChatHub> hubContext, IRepository<User> dataAccessLayer, EmailService emailService)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
            _hubContext = hubContext;
            _dataAccessLayer = dataAccessLayer;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            _logger.LogInformation("Get all Users");

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Add other actions such as GetById, Create, Update, Delete as needed

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.id = 0;
            user.verificationCode = new Random().Next(100000, 999999).ToString(); // Generate a 6-digit code
            user.isActivated = false;

            await _dataAccessLayer.AddAsync(user);

            // Send an activation email with the verification code
            await SendActivationEmail(user.email, user.verificationCode);

            return CreatedAtAction(nameof(user), new { id = user.id }, user);
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount(Guid id, string verificationCode)
        {
            var user = await _dataAccessLayer.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.verificationCode == verificationCode)
            {
                user.isActivated = true;
                await _dataAccessLayer.UpdateAsync(user);
                return Ok();
            }

            return BadRequest("Invalid verification code.");
        }

        private async Task SendActivationEmail(string email, string verificationCode)
        {
            string subject = "AI as a Service - Account Activation";
            string body = $"Your activation code is: {verificationCode}";

            await _emailService.SendEmailAsync(email, subject, body);
        }
    }
}
