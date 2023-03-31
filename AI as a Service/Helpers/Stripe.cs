namespace AI_as_a_Service.Helpers
{
    public class Stripe
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public Stripe(string apiKey)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.stripe.com/") };
            _apiKey = apiKey;
        }
    }
}
