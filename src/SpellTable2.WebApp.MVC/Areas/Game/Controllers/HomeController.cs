using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SpellTable2.Core.AutoMapping;
using SpellTable2.Services.Game;
using SpellTable2.WebApp.MVC.Areas.Game.Hubs;

namespace SpellTable2.WebApp.MVC.Areas.Game.Controllers
{
    [Area("Game")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<GameHub> _gameHubContext;

        public HomeController(ILogger<HomeController> logger, IService service, IMapper mapper, IHubContext<GameHub> gameHubContext)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _gameHubContext = gameHubContext;
        }

        public async Task<IActionResult> Index(Guid? id, Guid? userId)
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
                PlayerName = $"Player Name {userId.Value.ToString()[..5]}",
                LifeTotal = 40
            });

            await _gameHubContext.Clients.Group(gameInfo.GameId.ToString()).SendAsync("OnPlayerJoined", userId.Value);

            return View();
        }

        public IActionResult PlayerList(Guid id)
        {
            return ViewComponent("PlayerList", new { gameId = id});
        }
    }
}