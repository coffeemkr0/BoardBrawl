namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class PlayerBoard
    {
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        public List<PlayerInfo> Players { get; }

        public int FocusedPlayerId { get; set; }

        public int? ActivePlayerId { get; set; }

        public PlayerBoard()
        {
            Players = new List<PlayerInfo>();
        }
    }
}