using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Services.Game;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub<IGameHub>
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
            Context.Items.Add("GameId", gameId);
            Context.Items.Add("PlayerId", playerId);
            Context.Items.Add("PeerId", peerId);

            _logger.LogInformation($"Player joined game hub. gameId:{gameId}, playerId:{playerId}, peerId:{peerId}");

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());

            if (_service.PlayerIsNewOrChanged(Convert.ToInt32(gameId), Convert.ToInt32(playerId), peerId))
            {
                _service.UpdatePeerId(Convert.ToInt32(playerId), peerId);
                await Clients.GroupExcept(gameId.ToString(), Context.ConnectionId).OnPlayerJoined(playerId, peerId);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var gameId = Context.Items["GameId"].ToString();
            var playerId = Convert.ToInt32(Context.Items["PlayerId"]);

            _service.UpdatePeerId(Convert.ToInt32(playerId), Guid.Empty);
            Clients.GroupExcept(gameId, Context.ConnectionId).OnPlayerDisconnected(playerId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}