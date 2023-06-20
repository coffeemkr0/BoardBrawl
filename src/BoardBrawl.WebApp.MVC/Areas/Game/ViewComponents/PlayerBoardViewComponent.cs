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
            var model = new PlayerBoard { GameId = gameId, UserId = userId };
            var players = _service.GetPlayers(gameId);
            model.Players.AddRange(_mapper.Map<List<PlayerInfo>>(players));

            //TODO:Replace with real game state
            model.ActivePlayerUserId = model.Players.Count > 1 ? model.Players[1].UserId : model.Players.FirstOrDefault()?.UserId;

            return View(model);
        }
    }
}
