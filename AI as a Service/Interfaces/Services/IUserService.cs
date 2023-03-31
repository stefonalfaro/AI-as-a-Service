using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<bool> ActivateAccountAsync(int id, string verificationCode);
        Task<bool> ActivateAccountAsync(Guid id, string verificationCode);
        Task SendActivationEmailAsync(string email, string verificationCode);
    }
}