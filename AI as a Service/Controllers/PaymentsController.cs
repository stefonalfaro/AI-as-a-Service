using AI_as_a_Service.Helpers;
using AI_as_a_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly Configuration _configuration;
        Stripe _stripe;
        public PaymentsController(ILogger<PaymentsController> logger, Configuration configuration) 
        {
            _logger = logger;
            _configuration = configuration;
            _stripe = new Stripe(configuration.integrationSettings.StripeAPIKey);
        }
    }
}