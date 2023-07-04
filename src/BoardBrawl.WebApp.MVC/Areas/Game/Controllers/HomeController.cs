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
            var gameInfo = _service.GetGameInfo(id.Value, userId);
            if (gameInfo == null) { return Redirect("/Lobby"); }

            var servicePlayerInfo = _service.JoinGame(gameInfo.Id, userId);

            var model = new Model
            {
                GameId = id.Value,
                PlayerId = servicePlayerInfo.Id,
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
            var servicePlayerInfo = _service.AdjustLifeTotal(playerId, amount);
            var playerInfo = _mapper.Map<PlayerInfo>(servicePlayerInfo);
            playerInfo.IsSelf = servicePlayerInfo.UserId == userId;
            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerLifeTotalChanged", playerId);
            return PartialView("PlayerInfo/_PlayerLifeTotal", playerInfo);
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
                playerInfoViewModel.IsSelf = playerInfo.UserId == userId;
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

            var players = _service.GetPlayers(gameId);
            var myPlayer = players.First(i => i.UserId == userId);
            var focusedPlayer = players.FirstOrDefault(i => i.Id == myPlayer.FocusedPlayerId);

            playerBoard.GameId = gameId;
            playerBoard.PlayerId = myPlayer.Id;
            playerBoard.FocusedPlayerId = focusedPlayer?.Id ?? players.First().Id;
            playerBoard.ActivePlayerId = gameInfo.ActivePlayerId;

            playerBoard.Players.AddRange(_mapper.Map<List<PlayerInfo>>(players));
            playerBoard.Players.First(i => i.Id == myPlayer.Id).IsSelf = true;

            foreach (var player in playerBoard.Players)
            {
                await LoadCommanderCardInfoCommand.Execute(player);
            }
        }
    }
}