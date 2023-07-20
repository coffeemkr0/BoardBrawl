using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Services.Game;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly IService _service;

        public GameHub(ILogger<GameHub> logger, IService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task JoinGameHub(string gameId, string playerId, Guid peerId)
        {
            if (!_service.PlayerIsNewOrChanged(Convert.ToInt32(gameId),
                Convert.ToInt32(playerId), peerId)) return;

            _logger.LogInformation($"Player joined game hub. gameId:{gameId}, playerId:{playerId}, peerId:{peerId}");

            _service.UpdatePeerId(Convert.ToInt32(playerId), peerId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            await Clients.GroupExcept(gameId.ToString(), Context.ConnectionId).SendAsync("OnPlayerJoined", playerId, peerId);
        }
    }
}