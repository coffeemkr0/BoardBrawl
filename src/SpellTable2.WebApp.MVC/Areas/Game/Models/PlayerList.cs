namespace SpellTable2.WebApp.MVC.Areas.Game.Models
{
    public class PlayerList
    {
        public Guid GameId { get; set; }

        public List<PlayerInfo> Players { get; }

        public PlayerList()
        {
            Players = new List<PlayerInfo>();
        }
    }
}