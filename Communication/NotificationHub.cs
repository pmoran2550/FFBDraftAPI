using Microsoft.AspNetCore.SignalR;

namespace FFBDraftAPI.Communication
{
    public class NotificationHub : Hub
    {        public NotificationHub() { }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("PlayersUpdated", user, message);
        }
    }
}
