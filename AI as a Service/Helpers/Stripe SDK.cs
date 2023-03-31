using AI_as_a_Service.Models;
using Stripe;

namespace AI_as_a_Service.Helpers
{
    public class StripeSDK
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public StripeSDK(string apiKey)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.stripe.com/") };
            _apiKey = apiKey;
        }

        public async Task<Customer> CreateCustomerAsync(string email, string description = null)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                Description = description,
            };

            var service = new CustomerService();
            return await service.CreateAsync(options);
        }

        public async Task<Charge> CreateChargeAsync(string customerId, long amount, string currency = "usd", string description = null)
        {
            var options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Customer = customerId,
                Description = description,
            };

            var service = new ChargeService();
            return await service.CreateAsync(options);
        }

        public async Task<IEnumerable<Charge>> ListChargesAsync(int limit = 10)
        {
            var options = new ChargeListOptions
            {
                Limit = limit,
            };

            var service = new ChargeService();
            var charges = await service.ListAsync(options);
            return charges;
        }

        public async Task<Customer> CreateCustomerAsync(string email)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
            };

            var service = new CustomerService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentMethod> CreatePaymentMethodAsync(Payment payment)
        {
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = payment.CardNumber,
                    ExpMonth = payment.ExpMonth,
                    ExpYear = payment.ExpYear,
                    Cvc = payment.Cvc
                }
            };

            var service = new PaymentMethodService();
            return await service.CreateAsync(options);
        }

        public async Task AttachPaymentMethodToCustomerAsync(string customerId, string paymentMethodId)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = customerId,
            };

            var service = new PaymentMethodService();
            await service.AttachAsync(paymentMethodId, options);
        }

        public async Task<Subscription> CreateSubscriptionAsync(string customerId, string planId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = planId,
                    },
                },
            };

            var service = new SubscriptionService();
            return await service.CreateAsync(options);
        }
    }
}
