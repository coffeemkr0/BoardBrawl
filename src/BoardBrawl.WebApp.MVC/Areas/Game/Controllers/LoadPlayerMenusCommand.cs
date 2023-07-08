using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadPlayerMenusCommand
    {
        public static void Execute(Model model)
        {
            var myPlayer = model.PlayerBoard.Players.First(i => i.IsSelf);

            foreach (var player in model.PlayerBoard.Players)
            {
                player.PlayerMenu.CanBeMadeGameOwner = myPlayer.IsGameOwner && !player.IsSelf;
            }
        }
    }
}
