using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;
using Microsoft.AspNetCore.Identity;

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

        public IViewComponentResult Invoke(int gameId)
        {
            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var players = _service.GetPlayers(gameId);
            var myPlayer = players.First(i => i.UserId == userId);
            var focusedPlayer = players.FirstOrDefault(i => i.Id == myPlayer.FocusedPlayerId);

            var model = new PlayerBoard
            {
                GameId = gameId,
                UserId = userId,
                PlayerId = myPlayer.Id,
                FocusedPlayerId = focusedPlayer?.Id ?? players.First().Id
            };

            model.Players.AddRange(_mapper.Map<List<PlayerInfo>>(players));

            return View(model);
        }
    }
}
