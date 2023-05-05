﻿using Microsoft.AspNetCore.Mvc;
using SpellTable2.Services.Lobby;
using SpellTable2.Services.Lobby.Models;

namespace SpellTable2.WebApp.MVC.Areas.Lobby.Controllers
{
    [Area("Lobby")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService _service;

        public HomeController(ILogger<HomeController> logger, IService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            var newGame = new GameInfo
            {
                GameId = Guid.NewGuid(),
                Name = "Test Game",
                Description = "A game created by test code",
                IsPublic = true
            };

            _service.CreateGame(newGame);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.GameId });
        }
    }
}