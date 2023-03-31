namespace AI_as_a_Service.Middlewares
{
    // Create a Hubs folder and add a new class named ChatHub.cs
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}