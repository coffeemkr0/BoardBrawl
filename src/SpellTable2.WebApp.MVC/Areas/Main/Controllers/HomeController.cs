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
            //TODO:Simulate a random user Id that is stored in session data until we have Identity in place
            var userId = HttpContext.Session.GetString("user_id");
            if (userId == null)
            {
                HttpContext.Session.SetString("user_id", Guid.NewGuid().ToString());
            }

            return Redirect("Lobby");
        }
    }
}