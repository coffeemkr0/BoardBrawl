using Flurl.Http;
using Flurl;
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
            return View("Tooltips");
        }

        public async Task<IActionResult> SearchCommanders(string searchString)
        {
            try
            {
                // Make the API call using Flurl.Http
                var jsonResponse = await "https://api.scryfall.com/cards/search"
                    .SetQueryParams(new { q = searchString })
                    .GetJsonAsync();

                var results = new List<string>();

                foreach (var card in jsonResponse.data)
                {
                    results.Add(card.name);
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API call failed: {ex}");
                return NotFound();
            }
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