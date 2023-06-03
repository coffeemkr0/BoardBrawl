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
            var newGame = new Services.Lobby.Models.GameInfo
            {
                //TODO:Get game Id from service/repo
                GameId = Guid.NewGuid(),
                //TODO:Get UserId from Identity
                CreatedByUserId = gameInfo.CreatedByUserId,
                Name = gameInfo.Name,
                Description = gameInfo.Description,
                IsPublic = gameInfo.IsPublic
            };

            _service.CreateGame(newGame);

            return RedirectToAction("Index", "Home", new { area = "Game", id = newGame.GameId, userId = newGame.CreatedByUserId });
        }

        [HttpPost]
        public IActionResult JoinGame(Guid gameId, Guid userId)
        {
            //TODO:Get UserId from Identity

            return RedirectToAction("Index", "Home", new { area = "Game", id = gameId, userId });
        }

        [HttpPost]
        public IActionResult DeleteGame(Guid gameId, Guid userId)
        {
            //TODO:Get UserId from Identity

            return RedirectToAction("Index", new { userId = userId });
        }
    }
}