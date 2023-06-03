namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Models
{
    public class GameInfo
    {
        public Guid GameId { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PlayerCount { get; set; }
    }
}
