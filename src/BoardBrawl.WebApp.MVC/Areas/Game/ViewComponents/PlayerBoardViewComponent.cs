using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Microsoft.AspNetCore.Identity;
using BoardBrawl.WebApp.MVC.Areas.Game.Controllers;

namespace BoardBrawl.WebApp.MVC.Areas.Game.ViewComponents
{
    public class PlayerBoardViewComponent : ViewComponent
    {
        private readonly IService _service;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public PlayerBoardViewComponent(IService service, UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _service = service;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int gameId)
        {
            var gameInfo = _service.GetGameInfo(gameId);

            if (gameInfo == null) { throw new Exception($"Game not found with id {gameId}"); }

            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var players = _service.GetPlayers(gameId);
            var myPlayer = players.First(i => i.UserId == userId);
            var focusedPlayer = players.FirstOrDefault(i => i.Id == myPlayer.FocusedPlayerId);

            var model = new PlayerBoard
            {
                GameId = gameId,
                PlayerId = myPlayer.Id,
                FocusedPlayerId = focusedPlayer?.Id ?? players.First().Id,
                ActivePlayerId = gameInfo.ActivePlayerId
            };

            model.Players.AddRange(_mapper.Map<List<PlayerInfo>>(players));

            foreach (var player in model.Players)
            {
                await LoadCommanderCardInfoCommand.Execute(player);
            }

            return View(model);
        }
    }
}
