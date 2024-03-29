﻿using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application.Models;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Hubs;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using BoardBrawl.WebApp.MVC.Utils;
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
            if (!_service.IsPlayerInGame(id.Value, userId))
            {
                _service.JoinGame(id.Value, _userManager.GetUserId(User));
            }

            var model = await LoadModel(id.Value);

            return View(model);
        }

        public async Task<IActionResult> PlayerBoard(int gameId)
        {
            var model = await (LoadModel(gameId));
            return PartialView("_PlayerBoard", model.PlayerBoard);
        }

        public IActionResult UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            _service.UpdateFocusedPlayer(playerId, focusedPlayerId);
            return NoContent();
        }

        public async Task<IActionResult> AdjustLifeTotal(int gameId, int playerId, int amount)
        {
            _service.AdjustLifeTotal(playerId, amount);

            var model = await LoadModel(gameId);
            var newLifeTotal = model.PlayerBoard.Players.First(i=>i.Id == playerId).LifeTotal;

            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerLifeTotalChanged", playerId, newLifeTotal);

            return Ok();
        }

        public async Task<IActionResult> AdjustInfectCount(int gameId, int playerId, int amount)
        {
            _service.AdjustInfectCount(playerId, amount);

            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerInfectCountChanged", playerId, amount);

            return Ok();
        }

        public async Task<IActionResult> AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount)
        {
            _service.AdjustCommanderDamage(gameId, playerId, ownerPlayerId, cardId, amount);

            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnCommanderDamageChanged", playerId, ownerPlayerId, cardId, amount);

            return Ok();
        }

        public async Task<IActionResult> PromoteToGameOwner(int gameId, int playerId)
        {
            _service.PromoteToGameOwner(gameId, playerId);
            var redirectUrl = Url.Action("Index", "Home", new { area = "Game", id = gameId });

            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnGameOwnerChanged", redirectUrl);

            return Ok();
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
            _service.UpdateCommander(playerId, slot, cardId);

            await _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnCommanderChanged", playerId);

            return await GetCommanders(gameId, playerId);
        }

        public async Task<IActionResult> GetCommanders(int gameId, int playerId)
        {
            var model = await LoadModel(gameId);

            var commanderInfoPlayer = model.PlayerBoard.Players.First(i => i.Id == playerId);
            var commanderInfo = await this.RenderViewAsync("PlayerInfo/_CommanderInfo", commanderInfoPlayer, true);

            var commanderDamages = new Dictionary<int, string>();
            foreach (var player in model.PlayerBoard.Players)
            {
                commanderDamages.Add(player.Id, await this.RenderViewAsync("PlayerInfo/_CommanderDamages", player, true));
            }

            return Json(new { commanderInfo, commanderDamages });
        }

        public async Task<IActionResult> AddCardToCardHistory(int gameId, int playerId, string cardId)
        {
            _service.AddCardToCardHistory(gameId, playerId, cardId);

            var model = await LoadModel(gameId);

            return PartialView("_GamePanel", model);
        }

        public IActionResult RemoveCardFromCardHistory(int id)
        {
            _service.RemoveCardFromCardHistory(id);

            return Ok();
        }

        public async Task<IActionResult> AdjustPlayerTurnOrder([FromForm]PlayerTurnOrder playerTurnOrder)
        {
            _service.UpdatePlayerTurnOrder(playerTurnOrder.GameId, playerTurnOrder.Players.Select(i => i.Id).ToList());

            var model = await LoadModel(playerTurnOrder.GameId);
            return RedirectToAction("Index", new {id = playerTurnOrder.GameId});
        }

        public IActionResult LeaveGame(int gameId, int playerId)
        {
            _service.LeadGame(gameId, playerId);
            _gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnPlayerDisconnected", playerId);

            return Redirect("/Lobby");
        }

        public async Task<IActionResult> PassTurn(int gameId)
        {
            _service.PassTurn(Convert.ToInt32(gameId));

            var model = await LoadModel(gameId);

            await _gameHubContext.Clients.Group(gameId.ToString()).
                SendAsync("OnPassTurn", model.PlayerBoard.Players.First(i => i.IsActivePlayer).Id);

            return Ok();
        }

        private async Task<Model> LoadModel(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            var gameInfo = _service.GetGameInfo(gameId, userId);
            var myPlayer = gameInfo.Players.First(i => i.IsSelf);

            var model = new Model
            {
                GameId = gameInfo.Id,
                PlayerId = myPlayer.Id,
                GameName = gameInfo.Name
            };

            LoadGamePanelCommand.Execute(model, gameInfo, _mapper);
            LoadPlayerBoardCommand.Execute(model, gameInfo, _mapper);
            LoadPlayerMenusCommand.Execute(model);
            LoadSettingsMenuCommand.Execute(model);
            LoadCommanderDamageCommand.Execute(model);

            await LoadCardInfoCommand.Execute(model);

            return model;
        }
    }
}