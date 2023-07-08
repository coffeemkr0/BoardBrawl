namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class SettingsMenu
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public bool IsGameOwner { get; set; }
    }
}
