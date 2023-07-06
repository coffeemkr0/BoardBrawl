using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public class LoadCommanderDamageCommand
    {
        public static void Execute(Model model)
        {
            //Set the ReadOnly flag on all commander damages that don't belong to the current player
            foreach (var player in model.PlayerBoard.Players)
            {
                foreach (var playerId in player.CommanderDamages.Keys)
                {
                    foreach (var commanderDamage in player.CommanderDamages[playerId])
                    {
                        commanderDamage.ReadOnly = !player.IsSelf;
                    }
                }
            }
        }
    }
}
