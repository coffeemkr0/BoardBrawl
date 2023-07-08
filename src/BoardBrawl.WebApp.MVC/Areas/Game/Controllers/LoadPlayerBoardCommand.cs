using BoardBrawl.Core.AutoMapping;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadPlayerBoardCommand
    {
        public static void Execute(Model model, Services.Game.Models.GameInfo gameInfo, IMapper mapper)
        {
            var myPlayer = gameInfo.Players.First(i => i.IsSelf);
            var focusedPlayer = gameInfo.Players.FirstOrDefault(i => i.Id == myPlayer.FocusedPlayerId);

            model.PlayerBoard.GameId = gameInfo.Id;
            model.PlayerBoard.PlayerId = myPlayer.Id;
            model.PlayerBoard.FocusedPlayerId = focusedPlayer?.Id ?? gameInfo.Players.First().Id;
            model.PlayerBoard.ActivePlayerId = gameInfo.ActivePlayerId;

            model.PlayerBoard.Players.AddRange(mapper.Map<List<PlayerInfo>>(gameInfo.Players));
        }
    }
}
