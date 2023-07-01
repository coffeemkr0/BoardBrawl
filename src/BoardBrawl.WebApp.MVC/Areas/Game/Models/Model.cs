namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class Model
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public string GameName { get; set; }

        public PlayerBoard PlayerBoard { get; }

        public Model()
        {
            PlayerBoard = new PlayerBoard();
        }
    }
}