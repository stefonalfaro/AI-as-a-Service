using AI_as_a_Service.Data;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using AI_as_a_Service.Services.Interfaces;

namespace AI_as_a_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly Configuration _configuration;
        StripeSDK _stripe;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Payment> _dataAccessLayer;

        public PaymentsController(ILogger<PaymentsController> logger, Configuration configuration, IHubContext<ChatHub> hubContext, IRepository<Payment> dataAccessLayer)
        {
            _logger = logger;
            _configuration = configuration;
            _stripe = new StripeSDK(configuration.integrationSettings.StripeAPIKey);
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpPost("add-card")]
        public async Task<IActionResult> AddCardAsync([FromBody] Payment payment)
        {
            // Replace with the user's email from your authentication system
            string userEmail = "user@example.com";

            // Create a Stripe customer for the user if they don't already have one
            Customer customer = await _stripe.CreateCustomerAsync(userEmail);

            // Create a payment method with the user's credit card information
            PaymentMethod paymentMethod = await _stripe.CreatePaymentMethodAsync(payment);

            // Attach the payment method to the customer
            await _stripe.AttachPaymentMethodToCustomerAsync(customer.Id, paymentMethod.Id);

            // TODO: Save the customer ID and payment method ID in your database for future use

            return Ok();
        }


        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscriptionAsync(string planId)
        {
            // Replace with the customer ID from your database
            string customerId = "cus_123456789";

            // Create a subscription
            Subscription subscription = await _stripe.CreateSubscriptionAsync(customerId, planId);

            // TODO: Save the subscription ID in your database for future use

            return Ok();

        }
    }
}