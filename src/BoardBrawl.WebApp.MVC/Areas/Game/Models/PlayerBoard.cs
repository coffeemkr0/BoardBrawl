namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class PlayerBoard
    {
        public int GameId { get; set; }

        public Guid UserId { get; set; }

        public List<PlayerInfo> Players { get; }

        public Guid? ActivePlayerUserId { get; set; }

        public PlayerBoard()
        {
            Players = new List<PlayerInfo>();
        }
    }
}