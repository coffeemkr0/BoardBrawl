using Microsoft.AspNetCore.SignalR;

namespace SpellTable2.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinGameHub(string gameId, string userId, string peerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("OnPlayerJoined", userId, peerId);
        }
    }
}