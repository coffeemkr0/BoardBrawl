namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class GamePanel
    {
        public List<CardHistoryEntry> CardHistory { get; }

        public GamePanel()
        {
            CardHistory = new List<CardHistoryEntry>();
        }
    }
}