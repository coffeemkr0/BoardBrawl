using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace OpenCVTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            CardDetectionExample.CardDetector cardDetector = new CardDetectionExample.CardDetector();
            //cardDetector.DetectCard("c:\\temp\\test2.jpg");
            cardDetector.DetectCard(".\\images\\test2.jpg");
            cardDetector.DetectCard(".\\images\\test3.jpg");
        }
    }
}
