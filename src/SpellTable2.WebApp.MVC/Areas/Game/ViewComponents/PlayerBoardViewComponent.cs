using Microsoft.AspNetCore.Mvc;
using SpellTable2.Core.AutoMapping;
using SpellTable2.Services.Game;
using SpellTable2.WebApp.MVC.Areas.Game.Models;

namespace SpellTable2.WebApp.MVC.Areas.Game.ViewComponents
{
    public class PlayerBoardViewComponent : ViewComponent
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public PlayerBoardViewComponent(IService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke(Guid gameId, Guid userId)
        {
            var model = new PlayerBoard { GameId = gameId, UserId = userId };
            var players = _service.GetPlayers(gameId);
            model.Players.AddRange(_mapper.Map<List<PlayerInfo>>(players));

            return View(model);
        }
    }
}
