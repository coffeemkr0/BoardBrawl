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

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) { return Redirect("/Lobby"); }

            var userId = _userManager.GetUserId(User);
            _service.JoinGame(id.Value, userId);

            var gameInfo = _service.GetGameInfo(id.Value, userId);
            var myPlayer = gameInfo.Players.First(i => i.IsSelf);

            var model = new Model
            {
                GameId = id.Value,
                PlayerId = myPlayer.Id,
                GameName = gameInfo.Name
            };

            await LoadPlayerBoard(model.GameId, model.PlayerBoard);

            return View(model);
        }

        public async Task<IActionResult> PassTurn(int gameId)
        {
            _service.PassTurn(gameId);
            var playerBoard = new PlayerBoard();
            await LoadPlayerBoard(gameId, playerBoard);
            return PartialView("_PlayerBoard", playerBoard);
        }

        public async Task<IActionResult> PlayerBoard(int gameId)
        {
            var playerBoard = new PlayerBoard();
            await LoadPlayerBoard(gameId, playerBoard);
            return PartialView("_PlayerBoard", playerBoard);
        }

        public IActionResult UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            _service.UpdateFocusedPlayer(playerId, focusedPlayerId);
            return NoContent();
        }

        public async Task<IActionResult> AdjustLifeTotal(int gameId, int playerId, int amount)
        {
            var userId = _userManager.GetUserId(User);
            _service.AdjustLifeTotal(playerId, amount);
            var gameInfo = _service.GetGameInfo(gameId, userId);
            var playerInfo = gameInfo.Players.First(i=>i.Id == playerId);
            var playerInfoViewModel = _mapper.Map<PlayerInfo>(playerInfo);
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerLifeTotalChanged", playerId);
            return PartialView("PlayerInfo/_PlayerLifeTotal", playerInfoViewModel);
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

        public async Task<IActionResult> UpdateCommander(int gameId, int playerId, int slot, string cardId)
        {
            try
            {
                _service.UpdateCommander(playerId, slot, cardId);

                //TODO:This has to refresh all player infos to update commander damage tracking
                var userId = _userManager.GetUserId(User);
                var gameInfo = _service.GetGameInfo(gameId, userId);
                var playerInfo = gameInfo.Players.First(i => i.Id == playerId);

                var playerInfoViewModel = _mapper.Map<PlayerInfo>(playerInfo);
                await LoadCommanderCardInfoCommand.Execute(playerInfoViewModel);
                return PartialView("PlayerInfo/_CommanderInfo", playerInfoViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return NotFound();
            }
        }

        private async Task LoadPlayerBoard(int gameId, PlayerBoard playerBoard)
        {
            var userId = _userManager.GetUserId(User);
            var gameInfo = _service.GetGameInfo(gameId, userId);

            if (gameInfo == null) { throw new Exception($"Game not found with id {gameId}"); }

            var myPlayer = gameInfo.Players.First(i => i.IsSelf);
            var focusedPlayer = gameInfo.Players.FirstOrDefault(i => i.Id == myPlayer.FocusedPlayerId);

            playerBoard.GameId = gameId;
            playerBoard.PlayerId = myPlayer.Id;
            playerBoard.FocusedPlayerId = focusedPlayer?.Id ?? gameInfo.Players.First().Id;
            playerBoard.ActivePlayerId = gameInfo.ActivePlayerId;

            playerBoard.Players.AddRange(_mapper.Map<List<PlayerInfo>>(gameInfo.Players));

            foreach (var player in playerBoard.Players)
            {
                await LoadCommanderCardInfoCommand.Execute(player);
            }
        }
    }
}