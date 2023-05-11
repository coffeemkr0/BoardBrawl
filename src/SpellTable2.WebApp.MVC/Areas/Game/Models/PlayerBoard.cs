namespace SpellTable2.WebApp.MVC.Areas.Game.Models
{
    public class PlayerBoard
    {
        public Guid GameId { get; set; }

        public Guid UserId { get; set; }

        public List<PlayerInfo> Players { get; }

        public PlayerBoard()
        {
            Players = new List<PlayerInfo>();
        }
    }
}