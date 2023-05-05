using Microsoft.AspNetCore.SignalR;

namespace SpellTable2.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinGameHub(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }
    }
}