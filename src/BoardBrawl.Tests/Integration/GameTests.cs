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
            var lobbyGameInfo = new Services.Lobby.Models.GameInfo
            {
                Name = "Test Game",
                CreatedByUserId = "userId 1"
            };

            _lobbyService.CreateGame(lobbyGameInfo);

            var playerInfo = new Services.Game.Models.PlayerInfo
            {
                UserId = "user Id 1",
                Name = $"Test player",
                LifeTotal = 40
            };
            
            _gameService.AddPlayerToGame(lobbyGameInfo.Id, playerInfo);
        }
    }
}