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

        public IActionResult Index(Guid? id, Guid? userId)
        {
            if (userId == null) { return Redirect("/Main"); }
            if (id == null) { return Redirect("/Lobby"); }

            var gameInfo = _service.GetGameInfo(id.Value);
            if (gameInfo == null) { return Redirect("/Lobby"); }

            ViewBag.GameId = id.Value;
            ViewBag.GameName = gameInfo.Name;

            _service.AddPlayerToGame(id.Value, new Services.Game.Models.PlayerInfo
            {
                UserId = userId.Value,
                PlayerName = $"Player Name {userId.Value.ToString()[..5]}"
            });

            return View();
        }

        public IActionResult PlayerListPartial(Guid id)
        {
            var players = _service.GetPlayers(id);

            var playerList = new PlayerList();

            foreach (var playerInfo in players)
            {
                playerList.Players.Add(new PlayerInfo
                {
                    Name = playerInfo.PlayerName
                });
            }

            return PartialView("_PlayerList", playerList);
        }
    }
}