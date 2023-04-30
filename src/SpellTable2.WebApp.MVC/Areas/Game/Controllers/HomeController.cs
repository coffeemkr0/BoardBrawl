using Microsoft.AspNetCore.Mvc;
using SpellTable2.Services.Game;

namespace SpellTable2.WebApp.MVC.Areas.Game.Controllers
{
    [Area("Game")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService _service;

        public HomeController(ILogger<HomeController> logger, IService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index(Guid id)
        {
            var gameInfo = _service.GetGameInfo(id);
            
            var playerName = _service.GetPlayerName();
            if (string.IsNullOrEmpty(playerName))
            {
                _logger.LogInformation("Initializing player name");
                playerName = "Magic Mike";
                _service.SetPlayerName(playerName);
            }

            ViewBag.PlayerName = playerName;
            ViewBag.GameId = id;

            return View();
        }
    }
}