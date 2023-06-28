using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application.Models;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Hubs;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    [Authorize]
    [Area("Game")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<GameHub> _gameHubContext;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
            IService service, IMapper mapper, IHubContext<GameHub> gameHubContext)
        {
            _logger = logger;
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
            _gameHubContext = gameHubContext;
        }

        public IActionResult Index(int? id)
        {
            if (id == null) { return Redirect("/Lobby"); }

            var gameInfo = _service.GetGameInfo(id.Value);
            if (gameInfo == null) { return Redirect("/Lobby"); }

            var userId = _userManager.GetUserId(User);

            //TODO:Check to see if player is already in the game first, also consider a command to create the player info
            _service.AddPlayerToGame(id.Value, new Services.Game.Models.PlayerInfo
            {
                UserId = userId,
                Name = $"Player {userId.ToString()[..5]}",
                LifeTotal = 40
            });

            var model = new Model
            {
                GameId = id.Value,
                UserId = userId,
                GameName = gameInfo.Name
            };

            return View(model);
        }

        public IActionResult PassTurn(int gameId)
        {
            _service.PassTurn(gameId);

            var userId = _userManager.GetUserId(User);
            return ViewComponent("PlayerBoard", new { gameId, userId });
        }

        public IActionResult PlayerBoard(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            return ViewComponent("PlayerBoard", new { gameId, userId });
        }

        public async Task<IActionResult> PlayerInfo(int gameId, string userId)
        {
            //TODO:Get userId from Identity and get rid of gameId parameter
            var playerInfo = _mapper.Map<PlayerInfo>(_service.GetPlayer(userId));
            await LoadCommanderCardInfoCommand.Execute(playerInfo);
            return ViewComponent("PlayerInfo", playerInfo);
        }

        public IActionResult UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            _service.UpdateFocusedPlayer(playerId, focusedPlayerId);
            return NoContent();
        }

        public async Task<IActionResult> DecreaseLifeTotal(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseLifeTotal(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseLifeTotal(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseLifeTotal(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> DecreaseCommanderDamage(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseCommanderDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseCommanderDamage(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseCommanderDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> DecreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.DecreaseInfectDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> IncreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerInfo = _mapper.Map<PlayerInfo>(_service.IncreaseInfectDamage(gameId, userId, amount));
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
            return ViewComponent("PlayerInfo", new { playerInfo });
        }

        public async Task<IActionResult> SearchCards(string searchString)
        {
            try
            {
                var jsonResponse = await "https://api.scryfall.com/cards/search"
                    .SetQueryParams(new { q = searchString })
                    .GetJsonAsync();

                var results = new List<dynamic>();

                foreach (var card in jsonResponse.data)
                {
                    results.Add(new { label = card.name, value = card.id });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API call failed: {ex}");
                return NotFound();
            }
        }

        public async Task<IActionResult> UpdateCommander(int gameId, int slot, string cardId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var playerInfo = _service.GetPlayer(userId);
                switch (slot)
                {
                    case 1:
                        playerInfo.Commander1Id = cardId;
                        break;

                    case 2:
                        playerInfo.Commander2Id = cardId;
                        break;
                }
                _service.UpdatePlayerInfo(playerInfo);

                var playerInfoViewModel = _mapper.Map<PlayerInfo>(playerInfo);
                await LoadCommanderCardInfoCommand.Execute(playerInfoViewModel);
                await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfoChanged", userId);
                return ViewComponent("PlayerInfo", new { playerInfo = playerInfoViewModel });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return NotFound();
            }
        }
    }
}