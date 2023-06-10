using Microsoft.AspNetCore.Mvc;
using BoardBrawl.Services.Lobby;
using BoardBrawl.WebApp.MVC.Areas.Lobby.Models;
using BoardBrawl.Core.AutoMapping;

namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Controllers
{
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

            var newGame = new Services.Lobby.Models.GameInfo
            {
                CreatedByUserId = userId,
                Name = gameInfo.Name,
                Description = gameInfo.Description,
                IsPublic = gameInfo.IsPublic
            };

            _service.CreateGame(newGame);
            _service.JoinGame(newGame.Id, userId);

            return RedirectToAction("Index", "Home", new { area = "Lobby", userId = userId });
            //return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.Id, userId = userId });
        }

        
        public IActionResult JoinGame(int gameId, Guid userId)
        {
            //TODO:Get UserId from Identity
            _service.JoinGame(gameId, userId);
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