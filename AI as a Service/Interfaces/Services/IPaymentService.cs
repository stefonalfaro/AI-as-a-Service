using AI_as_a_Service.Models;

namespace AI_as_a_Service.Interfaces.Services
{
    public interface IPaymentService
    {
        Task AddCardAsync(Payment payment, string userEmail);
        Task CreateSubscriptionAsync(string customerId, string planId);
    }
}