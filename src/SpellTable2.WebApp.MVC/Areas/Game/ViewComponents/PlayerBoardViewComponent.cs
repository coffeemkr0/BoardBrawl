﻿using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Services.Game;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.ViewComponents
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

            //TODO:Replace with real game state
            model.ActivePlayerUserId = model.Players.Count>1? model.Players[1].UserId : model.Players.FirstOrDefault()?.UserId;

            return View(model);
        }
    }
}
