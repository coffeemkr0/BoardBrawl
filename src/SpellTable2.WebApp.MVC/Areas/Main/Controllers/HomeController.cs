using Microsoft.AspNetCore.Mvc;

namespace SpellTable2.WebApp.MVC.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //TODO:Simulate a random user Id until we have Identity in place
            var userId = Guid.NewGuid().ToString();

            return RedirectToAction("Index", "Home", new { area = "Lobby", userId });
        }
    }
}