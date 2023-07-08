namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class PlayerTurnOrder
    {
        public int GameId { get; set; }

        public List<Player> Players { get; }

        public PlayerTurnOrder()
        {
            Players = new List<Player>();
        }
    }

    public class Player
    {
        public int Id { get; set; }

        public string PlayerName { get; set; }
    }
}
