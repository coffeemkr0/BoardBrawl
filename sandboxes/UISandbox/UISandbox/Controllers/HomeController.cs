using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UISandbox.Models;

namespace UISandbox.Controllers
{
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

        public JsonResult SearchCommanders(string searchString)
        {
            var commanders = new List<string>
            {
                "Commander 1",
                "Test 1"
            };

            return Json(commanders);
        }

        public IActionResult SelectCommander(string name)
        {
            return Redirect("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}