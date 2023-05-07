using Microsoft.AspNetCore.Mvc;
using SpellTable2.Core.AutoMapping;
using SpellTable2.Services.Game;
using SpellTable2.WebApp.MVC.Areas.Game.Models;

namespace SpellTable2.WebApp.MVC.Areas.Game.ViewComponents
{
    public class PlayerListViewComponent : ViewComponent
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public PlayerListViewComponent(IService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke(Guid gameId, Guid userId)
        {
            var playerList = new PlayerList { GameId = gameId, UserId = userId };

            playerList.Players.AddRange(_mapper.Map<List<PlayerInfo>>(_service.GetPlayers(gameId)));

            return View(playerList);
        }
    }
}
