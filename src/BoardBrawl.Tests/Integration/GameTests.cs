using BoardBrawl.Data.Application;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using BoardBrawl.WebApp.MVC.AutoMapping;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Tests.Integration
{
    [TestClass]
    public class GameTests
    {
        const string CardId_TheUrDragon = "10d42b35-844f-4a64-9981-c6118d45e826";

        private Services.Game.IService _gameService;
        private Services.Lobby.IService _lobbyService;

        [TestInitialize]
        public void Init()
        {
            var connectionString = "Server=localhost;Database=BoardBrawl;Uid=root;";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("10.11.3"));
            var appDbContext = new ApplicationDbContext(optionsBuilder.Options);
            var mapper = new AutoMapperMapper();

            var gameRepo = new Repositories.Game.EntityFrameworkRepository(appDbContext, mapper);
            var lobbyRepo = new Repositories.Lobby.EntityFrameworkRepository(mapper, appDbContext);

            _gameService = new Services.Game.Service(gameRepo, mapper);
            _lobbyService = new BoardBrawl.Services.Lobby.Service(lobbyRepo, mapper);
        }

        [TestMethod]
        public void TestUpdateCommanders()
        {
            var userId = Guid.NewGuid().ToString();

            //Create a game
            var lobbyGameInfo = new Services.Lobby.Models.GameInfo
            {
                Name = "Test Game",
                OwnerUserId = userId
            };

            _lobbyService.CreateGame(lobbyGameInfo);

            //Join the game
            _gameService.JoinGame(lobbyGameInfo.Id, userId);

            //Get the updated game
            var gameInfo = _gameService.GetGameInfo(lobbyGameInfo.Id, userId);
            var myPlayer = gameInfo.Players.First(i => i.IsSelf);

            //Add a commander to the player
            _gameService.UpdateCommander(myPlayer.Id, 1, CardId_TheUrDragon);

            //Make sure the player info comes back correctly
            gameInfo = _gameService.GetGameInfo(lobbyGameInfo.Id, userId);
            myPlayer = gameInfo.Players.First(i => i.IsSelf);

            Assert.IsTrue(myPlayer.Commander1Id == CardId_TheUrDragon);
        }

        [TestMethod]
        public void TestCreateGameAndGetGame()
        {
            var userId = Guid.NewGuid().ToString();

            //Create a game
            var lobbyGameInfo = new Services.Lobby.Models.GameInfo
            {
                Name = "Test Game",
                OwnerUserId = userId
            };

            _lobbyService.CreateGame(lobbyGameInfo);

            //Join the game
            _gameService.JoinGame(lobbyGameInfo.Id, userId);

            //Get the updated game
            var game = _gameService.GetGameInfo(lobbyGameInfo.Id, userId);
            var myPlayer = game.Players.First(i => i.IsSelf);

            //Make sure the player info comes back correctly
            game = _gameService.GetGameInfo(lobbyGameInfo.Id, userId);
            myPlayer = game.Players.First(i => i.IsSelf);

            Assert.IsTrue(game.Players.Count() == 1);
            Assert.AreEqual(game.Players.First().Id, myPlayer.Id);
            Assert.IsTrue(game.Players.First().IsSelf);
        }
    }
}