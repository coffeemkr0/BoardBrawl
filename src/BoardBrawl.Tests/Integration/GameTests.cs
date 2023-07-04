using BoardBrawl.Data.Application;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using BoardBrawl.WebApp.MVC.AutoMapping;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Tests.Integration
{
    [TestClass]
    public class GameTests
    {
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
            //Create a game
            var lobbyGameInfo = new Services.Lobby.Models.GameInfo
            {
                Name = "Test Game",
                CreatedByUserId = "userId 1"
            };

            _lobbyService.CreateGame(lobbyGameInfo);

            //Join the game
            var playerInfo = _gameService.JoinGame(lobbyGameInfo.Id, "userId 1");

            //Add a commander to the player
            var commanderId = Guid.NewGuid().ToString();
            playerInfo.Commander1Id = commanderId;
            _gameService.UpdatePlayerInfo(playerInfo);

            //Make sure the player info comes back correctly
            playerInfo = _gameService.GetPlayer(playerInfo.UserId);

            Assert.IsTrue(playerInfo.Commander1Id == commanderId);
        }

        [TestMethod]
        public void TestCreateGameAndGetGame()
        {
            var userId = Guid.NewGuid().ToString();

            //Create a game
            var lobbyGameInfo = new Services.Lobby.Models.GameInfo
            {
                Name = "Test Game",
                CreatedByUserId = userId
            };

            _lobbyService.CreateGame(lobbyGameInfo);

            //Join the game
            var playerInfo = _gameService.JoinGame(lobbyGameInfo.Id, userId);

            //Add a commander to the player
            var commanderId = Guid.NewGuid().ToString();
            playerInfo.Commander1Id = commanderId;
            _gameService.UpdatePlayerInfo(playerInfo);

            //Make sure the player info comes back correctly
            var game = _gameService.GetGameInfo(lobbyGameInfo.Id, userId);

            Assert.IsTrue(game.Players.Count() == 1);
            Assert.AreEqual(game.Players.First().Id, playerInfo.Id);
            Assert.IsTrue(game.Players.First().IsSelf);
        }
    }
}