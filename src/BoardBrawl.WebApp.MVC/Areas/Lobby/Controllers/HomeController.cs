using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
using BoardBrawl.Services.Lobby.Models;
using BoardBrawl.WebApp.MVC.Areas.Lobby.Models;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGame([FromForm] NewGameInfo newGameInfo)
        {
            var userId = Guid.NewGuid();

            var newGame = new GameInfo
            {
                GameId = Guid.NewGuid(),
                Name = newGameInfo.GameName,
                Description = newGameInfo.GameDescription,
                IsPublic = newGameInfo.IsPublic
            };

            _service.CreateGame(newGame);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.GameId, userId });
        }
    }
}