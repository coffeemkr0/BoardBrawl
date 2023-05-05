using Microsoft.AspNetCore.Mvc;
using SpellTable2.Services.Game;
using SpellTable2.WebApp.MVC.Areas.Game.Models;

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

        public IActionResult Index(Guid? gameId, Guid? userId)
        {
            if (userId == null) { return Redirect("/Main"); }
            if (gameId == null) { return Redirect("/Lobby"); }

            var gameInfo = _service.GetGameInfo(gameId.Value);
            if (gameInfo == null) { return Redirect("/Lobby"); }

            ViewBag.GameId = gameId.Value;
            ViewBag.GameName = gameInfo.Name;
            ViewBag.UserId = userId;

            return View();
        }

        public IActionResult PlayerListPartial()
        {
            var playerList = new PlayerList();

            playerList.Players.AddRange(new List<PlayerInfo>
            { 
                new PlayerInfo { Name = "Player 1" },
                new PlayerInfo { Name = "Player 2" },
                new PlayerInfo { Name = "Player 3" },
                new PlayerInfo { Name = "Player 4" }
            });

            return PartialView("_PlayerList", playerList);
        }
    }
}