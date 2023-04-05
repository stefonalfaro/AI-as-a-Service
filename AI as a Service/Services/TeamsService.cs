using AI_as_a_Service.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Services
{
    public class TeamsService
    {
        public TeamsService() { 

        }

        public async void initialize()
        {
            string clientId = "your_client_id";
            string tenantId = "your_tenant_id";
            string clientSecret = "your_client_secret";

            TeamsSDK teamsSDK = new TeamsSDK(clientId, tenantId, clientSecret);
            //var messages = await teamsSDK.GetChannelMessagesAsync("your_team_id", "your_channel_id");
        }
    }
}
