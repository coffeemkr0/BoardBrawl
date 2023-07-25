using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Services.Game;
using System.Linq;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public class GameHub : Hub<IGameHub>
    {
        private readonly Dictionary<int, string> _playerConnections;
        private readonly ILogger<GameHub> _logger;
        private readonly IService _service;

        public GameHub(ILogger<GameHub> logger, IService service)
        {
            _playerConnections = new Dictionary<int, string>();

            _logger = logger;
            _service = service;
        }

        public async Task JoinGameHub(int gameId, int playerId, Guid peerId)
        {
            if (_playerConnections.ContainsKey(playerId))
            {
                _playerConnections[playerId] = Context.ConnectionId;
            }
            else
            {
                _playerConnections.Add(playerId, Context.ConnectionId);
            }

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

        //public async Task<string> GetNewPeerId(int playerId)
        //{
        //    var connectionId = _playerConnections[playerId];
        //    return await Clients.Client(connectionId).GetNewPeerId();
        //}

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var gameId = Context.Items["GameId"].ToString();
            var playerId = Convert.ToInt32(Context.Items["PlayerId"]);

            if (_playerConnections.ContainsKey(playerId))
            {
                _playerConnections.Remove(playerId);
            }

            _service.UpdatePeerId(Convert.ToInt32(playerId), Guid.Empty);

            Clients.GroupExcept(gameId, Context.ConnectionId).OnPlayerDisconnected(playerId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}