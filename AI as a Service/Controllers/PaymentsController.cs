using AI_as_a_Service.Data;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Interfaces.Services;

namespace AI_as_a_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost("add-card")]
        public async Task<IActionResult> AddCardAsync([FromBody] Payment payment)
        {
            // Replace with the user's email from your authentication system
            string userEmail = "user@example.com";

            await _paymentService.AddCardAsync(payment, userEmail);

            return Ok();
        }

        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscriptionAsync(string planId)
        {
            // Replace with the customer ID from your database
            string customerId = "cus_123456789";

            await _paymentService.CreateSubscriptionAsync(customerId, planId);

            return Ok();
        }
    }

}