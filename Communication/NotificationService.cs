using Microsoft.AspNetCore.SignalR;

namespace FFBDraftAPI.Communication
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAll(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("PlayersUpdated", user, message);
        }
    }
}
