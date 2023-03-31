namespace AI_as_a_Service.Models
{
    public class Configuration
    {
        private static Configuration _instance;

        public int FreemiumTimer { get; set; }
        public bool DisableAllLogin { get; set; }
        public IntegrationSettings? integrationSettings { get; set; }

        private Configuration()
        {
            // Set default values or load from a file or a database.
            FreemiumTimer = 60;
            DisableAllLogin = false;
        }

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
                return _instance;
            }
        }

        public class IntegrationSettings
        {
            public string? StripeAPIKey { get; set; }
            public string? OpenAPIKey { get; set; }
        }
    }
}