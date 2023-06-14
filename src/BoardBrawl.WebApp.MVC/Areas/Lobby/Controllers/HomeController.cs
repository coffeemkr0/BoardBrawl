using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
using BoardBrawl.WebApp.MVC.Areas.Lobby.Models;
using BoardBrawl.Core.AutoMapping;
using Microsoft.AspNetCore.Authorization;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Controllers
{
    [Authorize]
    [Area("Lobby")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService _service;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index(Guid? userId)
        {
            //TODO:Get UserId from Identity
            if (userId == null) { return Redirect("/Main"); }

            var model = new Model
            {
                UserId = userId.Value
            };

            model.MyGames.AddRange(_mapper.Map<List<GameInfo>>(_service.GetGames(userId.Value)));

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateGame([FromForm] GameInfo gameInfo)
        {
            //TODO:Get UserId from Identity
            var userId = gameInfo.CreatedByUserId;

            var newGame = _mapper.Map<Services.Lobby.Models.GameInfo>(gameInfo);

            _service.CreateGame(newGame);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.Id, userId = userId });
        }

        
        public IActionResult JoinGame(int gameId, Guid userId)
        {
            //TODO:Get UserId from Identity
            return RedirectToAction("Index", "Home", new { area = "Game", id = gameId, userId });
        }

        [HttpPost]
        public IActionResult DeleteGame(int gameId, Guid userId)
        {
            //TODO:Get UserId from Identity
            _service.DeleteGame(gameId);
            return RedirectToAction("Index", new { userId = userId });
        }
    }
}