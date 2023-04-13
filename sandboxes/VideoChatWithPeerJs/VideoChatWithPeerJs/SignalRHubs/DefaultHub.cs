using Microsoft.AspNetCore.SignalR;

namespace VideoChatWithPeerJs.SignalRHubs
{
    public class DefaultHub : Hub
    {
        public async Task JoinRoom(string roomId, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("user-connected", userId);
        }
    }
}