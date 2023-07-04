using BoardBrawl.Repositories.Game;
using BoardBrawl.Services.Game;

namespace BoardBrawl.Services.Test
{
    public class TestService
    {
        private readonly IService _gameService;
        private readonly IRepository _gameRepository;

        public TestService(IService gameService, IRepository gameRepository)
        {
            _gameService = gameService;
            _gameRepository = gameRepository;
        }

        public void AddTestPlayersToGame(int gameId)
        {
            //Join as a new player
            var userId = Guid.NewGuid().ToString();
            _gameService.JoinGame(gameId, userId);

            //Add a commander to the player (The Ur Dragon)
            var game = _gameService.GetGameInfo(gameId, userId);
            var testPlayer = game.Players.First(i => i.UserId == userId);
            _gameService.UpdateCommander(testPlayer.Id, 1, "10d42b35-844f-4a64-9981-c6118d45e826");
        }
    }
}
