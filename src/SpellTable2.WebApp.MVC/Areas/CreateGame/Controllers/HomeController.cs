using Microsoft.AspNetCore.Mvc;

namespace SpellTable2.WebApp.MVC.Areas.CreateGame.Controllers
{
    [Area("CreateGame")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}