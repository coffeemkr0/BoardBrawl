using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadSettingsMenuCommand
    {
        public static void Execute(Model model)
        {
            var myPlayer = model.PlayerBoard.Players.First(i => i.IsSelf);

            model.SettingsMenu.GameId = model.GameId;
            model.SettingsMenu.PlayerId = myPlayer.Id;
            model.SettingsMenu.IsGameOwner = myPlayer.IsGameOwner;
        }
    }
}