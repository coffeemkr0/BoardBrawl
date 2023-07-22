using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
using BoardBrawl.WebApp.MVC.Areas.Lobby.Models;
using BoardBrawl.Core.AutoMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BoardBrawl.Services.Test;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Controllers
{
    [Authorize]
    [Area("Lobby")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly TestService _testService;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
            IService service, IMapper mapper, TestService testService)
        {
            _logger = logger;
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
            _testService = testService;
        }

        public IActionResult Index()
        {
            var model = new Model
            {
                UserId = _userManager.GetUserId(User)
            };

            model.MyGames.AddRange(_mapper.Map<List<GameInfo>>(_service.GetGames(model.UserId)));

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateGame([FromForm] GameInfo gameInfo)
        {
            var userId = _userManager.GetUserId(User);

            var newGame = _mapper.Map<Services.Lobby.Models.GameInfo>(gameInfo);
            newGame.OwnerUserId = userId;

            _service.CreateGame(newGame);
            //_testService.AddTestPlayersToGame(newGame.Id);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.Id });
        }

        
        public IActionResult JoinGame(int gameId)
        {
            return RedirectToAction("Index", "Home", new { area = "Game", id = gameId });
        }

        [HttpPost]
        public IActionResult DeleteGame(int gameId)
        {
            _service.DeleteGame(gameId);
            return RedirectToAction("Index");
        }
    }
}