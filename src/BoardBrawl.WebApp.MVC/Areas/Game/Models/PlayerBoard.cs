namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class PlayerBoard
    {
        public int GameId { get; set; }

        public string UserId { get; set; }

        public List<PlayerInfo> Players { get; }

        public string ActivePlayerUserId { get; set; }

        public PlayerBoard()
        {
            Players = new List<PlayerInfo>();
        }
    }
}