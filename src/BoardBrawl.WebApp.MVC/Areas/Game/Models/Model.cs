namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class Model
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public string GameName { get; set; }

        public PlayerBoard PlayerBoard { get; }

        public SettingsMenu SettingsMenu { get; }

        public GamePanel GamePanel { get; }

        public Model()
        {
            PlayerBoard = new PlayerBoard();
            SettingsMenu = new SettingsMenu();
            GamePanel = new GamePanel();
        }
    }
}