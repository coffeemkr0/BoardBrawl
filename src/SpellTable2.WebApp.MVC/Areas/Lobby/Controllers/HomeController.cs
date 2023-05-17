using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
using BoardBrawl.Services.Lobby.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Controllers
{
    [Area("Lobby")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService _service;

        public HomeController(ILogger<HomeController> logger, IService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index(Guid? userId)
        {
            if (userId == null) { return Redirect("/Main"); }

            Guid gameIdToJoin;

            var publicGames = _service.GetPublicGames();

            if(!publicGames.Any())
            {
                var newGame = new GameInfo
                {
                    GameId = Guid.NewGuid(),
                    Name = "Test Game",
                    Description = "A game created by test code",
                    IsPublic = true
                };

                _service.CreateGame(newGame);

                gameIdToJoin = newGame.GameId;
            }
            else
            {
                gameIdToJoin = publicGames.First().GameId;
            }

            return RedirectToAction("Index", "Home", new { area = "Game", id = gameIdToJoin, userId });
        }
    }
}