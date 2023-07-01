using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Services.Game;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub
    {
        private readonly IService _service;

        public GameHub(IService service)
        {
            _service = service;
        }

        public async Task JoinGameHub(string gameId, string playerId, Guid peerId)
        {
            _service.UpdatePeerId(Convert.ToInt32(playerId), peerId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            await Clients.GroupExcept(gameId.ToString(), Context.ConnectionId).SendAsync("OnPlayerJoined", playerId, peerId);
        }

        public async Task PassTurn(string gameId)
        {
            //TODO:This doesn't really need to be initiated by a hub call - call the controller and let the controller signal the clients
            //See how life total changes are handled
            _service.PassTurn(Convert.ToInt32(gameId));
            await Clients.Group(gameId.ToString()).SendAsync("OnPassTurn");
        }
    }
}