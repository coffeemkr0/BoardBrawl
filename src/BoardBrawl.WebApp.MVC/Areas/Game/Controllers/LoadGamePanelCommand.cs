using BoardBrawl.Core.AutoMapping;
using BoardBrawl.WebApp.MVC.Areas.Game.Models;

namespace BoardBrawl.WebApp.MVC.Areas.Game.Controllers
{
    public static class LoadGamePanelCommand
    {
        public static void Execute(Model model, Services.Game.Models.GameInfo gameInfo, IMapper mapper)
        {
            var cardHistory = mapper.Map<List<CardHistoryEntry>>(gameInfo.CardHistory);
            model.GamePanel.LastCard = cardHistory.FirstOrDefault();
            model.GamePanel.CardHistory.AddRange(cardHistory.Skip(1));
        }
    }
}