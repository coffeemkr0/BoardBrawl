using Microsoft.AspNetCore.Mvc;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.ViewComponents
{
    public class PlayerInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PlayerInfo playerInfo)
        {
            return View(playerInfo);
        }
    }
}