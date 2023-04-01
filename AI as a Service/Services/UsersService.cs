using AI_as_a_Service.Controllers;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AI_as_a_Service.Services
{
    public class UsersService : IUserService
    {
        private readonly IRepository<User> _dataAccessLayer;
        private readonly EmailService _emailService;
        private readonly IHubContext<ChatHub> _stateManagement;
        private readonly Configuration _configuration;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IRepository<User> dataAccessLayer, EmailService emailService, IHubContext<ChatHub> stateManagement, Configuration configuration, ILogger<UsersService> logger)
        {
            _dataAccessLayer = dataAccessLayer;
            _emailService = emailService;
            _stateManagement = stateManagement;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dataAccessLayer.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _dataAccessLayer.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.id = 0;
            user.verificationCode = new Random().Next(100000, 999999).ToString(); // Generate a 6-digit code
            user.isActivated = false;

            await _dataAccessLayer.AddAsync(user);
            await SendActivationEmailAsync(user.email, user.verificationCode);
            await _stateManagement.Clients.All.SendAsync("UserCreated", user);

            return user;
        }

        public async Task<bool> ActivateAccountAsync(int id, string verificationCode)
        {
            var user = await _dataAccessLayer.GetByIdAsync(id);

            if (user != null && user.verificationCode == verificationCode)
            {
                user.isActivated = true;
                await _dataAccessLayer.UpdateAsync(user);
                await _stateManagement.Clients.All.SendAsync("UserActivated", user);

                return true;
            }

            return false;
        }

        public async Task SendActivationEmailAsync(string email, string verificationCode)
        {
            string subject = "AI as a Service - Account Activation";
            string body = $"Your activation code is: {verificationCode}";

            await _emailService.SendEmailAsync(email, subject, body);
        }

        Task<User> IUserService.GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.ActivateAccountAsync(Guid id, string verificationCode)
        {
            throw new NotImplementedException();
        }
    }
}
