using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
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
            var model = new Model
            {
                //TODO:Get UserId from Identity
                UserId = Guid.NewGuid()
            };

            model.MyGames.Add(new GameInfo 
            { 
                GameId = Guid.NewGuid(),
                Name = "Game 1",
                Description = "A hard coded test game",
                IsPublic = false,
                PlayerCount = 1
            });

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateGame([FromForm] GameInfo gameInfo)
        {
            //TODO:Get UserId from Identity
            var userId = Guid.NewGuid();

            var newGame = new Services.Lobby.Models.GameInfo
            {
                //TODO:Get game Id from service/repo
                GameId = Guid.NewGuid(),
                Name = gameInfo.Name,
                Description = gameInfo.Description,
                IsPublic = gameInfo.IsPublic
            };

            _service.CreateGame(newGame);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.GameId, userId });
        }
    }
}