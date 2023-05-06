using Microsoft.AspNetCore.Mvc;
using SpellTable2.WebApp.MVC.Areas.Game.Models;

namespace SpellTable2.WebApp.MVC.Areas.Game.ViewComponents
{
    public class PlayerInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PlayerInfo playerInfo)
        {
            return View(playerInfo);
        }
    }
}