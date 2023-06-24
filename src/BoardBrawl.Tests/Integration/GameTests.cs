using BoardBrawl.Data.Application;
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

            //Add a player to the game
            var playerInfo = new Services.Game.Models.PlayerInfo
            {
                UserId = "user Id 1",
                Name = $"Test player",
                LifeTotal = 40
            };
            
            _gameService.AddPlayerToGame(lobbyGameInfo.Id, playerInfo);

            //Add a commander to the player
            playerInfo = _gameService.GetPlayers(lobbyGameInfo.Id).First();

            playerInfo.Commanders.Add(new Services.Game.Models.Commander
            {
                Name = "Commander 1",
                ImageUri = ""
            });
            playerInfo.Commanders.Last().Colors.Add(Repositories.Game.Models.Colors.Red);
            playerInfo.Commanders.Last().Colors.Add(Repositories.Game.Models.Colors.White);

            _gameService.UpdateCommanders(playerInfo.Id, playerInfo.Commanders);

            //Make sure the player info comes back correctly
            playerInfo = _gameService.GetPlayers(lobbyGameInfo.Id).First();
            Assert.IsTrue(playerInfo.Commanders.First().Colors[1] == Repositories.Game.Models.Colors.White);
        }
    }
}