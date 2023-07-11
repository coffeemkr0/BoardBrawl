namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class GamePanel
    {
        public CardHistoryEntry LastCard { get; set; }

        public List<CardHistoryEntry> CardHistory { get; }

        public GamePanel()
        {
            CardHistory = new List<CardHistoryEntry>();
        }
    }
}