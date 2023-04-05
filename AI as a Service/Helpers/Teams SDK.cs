using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AI_as_a_Service.Helpers
{
    public class TeamsSDK
    {
        private readonly string _clientId;
        private readonly string _tenantId;
        private readonly string _clientSecret;
        private readonly string _authority;
        private readonly string[] _scopes = new[] { "https://graph.microsoft.com/.default" };

        public TeamsSDK(string clientId, string tenantId, string clientSecret)
        {
            _clientId = clientId;
            _tenantId = tenantId;
            _clientSecret = clientSecret;
            _authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
        }
    }
}
