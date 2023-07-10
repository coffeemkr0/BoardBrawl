namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class GamePanel
    {
        public CardHistoryEntry LastCard { get; }

        public List<CardHistoryEntry> CardHistory { get; }

        public GamePanel()
        {
            LastCard = new CardHistoryEntry();
            CardHistory = new List<CardHistoryEntry>();
        }
    }
}