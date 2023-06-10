using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Hubs;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Microsoft.AspNetCore.Authorization;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
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

        public IActionResult Index(int? id, Guid? userId)
        {
            if (userId == null) { return Redirect("/Main"); }
            if (id == null) { return Redirect("/Lobby"); }

            var gameInfo = _service.GetGameInfo(id.Value);
            if (gameInfo == null) { return Redirect("/Lobby"); }

            _service.AddPlayerToGame(id.Value, new Services.Game.Models.PlayerInfo
            {
                UserId = userId.Value,
                Name = $"Player {userId.Value.ToString()[..5]}",
                LifeTotal = 40
            });

            var model = new Model
            {
                GameId = id.Value,
                UserId = userId.Value,
                GameName = gameInfo.Name
            };

            return View(model);
        }

        public IActionResult Clear()
        {
            _service.ClearPlayers();
            return Redirect("Index");
        }


        public IActionResult PlayerBoard(int gameId, Guid userId)
        {
            return ViewComponent("PlayerBoard", new { gameId, userId });
        }

        public IActionResult PlayerInfo(int gameId, Guid userId)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.GetPlayers(gameId).First(i => i.UserId == userId));
            return ViewComponent("PlayerInfo", playerInfo);
        }

        public async Task<IActionResult> DecreaseLifeTotal(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseLifeTotal(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseLifeTotal(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseLifeTotal(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> DecreaseCommanderDamage(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseCommanderDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseCommanderDamage(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseCommanderDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> DecreaseInfectDamage(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseInfectDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseInfectDamage(int gameId, Guid userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseInfectDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }
    }
}