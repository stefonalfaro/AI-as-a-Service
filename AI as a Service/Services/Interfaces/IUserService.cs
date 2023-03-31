using AI_as_a_Service.Models;

namespace AI_as_a_Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        // Add other methods such as GetById, Create, Update, Delete as needed
    }
}
