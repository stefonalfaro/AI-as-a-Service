using AI_as_a_Service.Helpers;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Models;
using Stripe;

namespace AI_as_a_Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly Configuration _configuration;
        private readonly StripeSDK _stripe;

        public PaymentService(Configuration configuration)
        {
            _configuration = configuration;
            _stripe = new StripeSDK(configuration.integrationSettings.StripeAPIKey);
        }

        public async Task AddCardAsync(Payment payment, string userEmail)
        {
            // Create a Stripe customer for the user if they don't already have one
            Customer customer = await _stripe.CreateCustomerAsync(userEmail);

            // Create a payment method with the user's credit card information
            PaymentMethod paymentMethod = await _stripe.CreatePaymentMethodAsync(payment);

            // Attach the payment method to the customer
            await _stripe.AttachPaymentMethodToCustomerAsync(customer.Id, paymentMethod.Id);

            // TODO: Save the customer ID and payment method ID in your database for future use
        }

        public async Task CreateSubscriptionAsync(string customerId, string planId)
        {
            // Create a subscription
            Subscription subscription = await _stripe.CreateSubscriptionAsync(customerId, planId);

            // TODO: Save the subscription ID in your database for future use
        }
    }

}
