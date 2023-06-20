namespace BoardBrawl.Repositories.Lobby.Models
{
    public class GameInfo
    {
        public int Id { get; set; }
        public string CreatedByUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public int PlayerCount { get; set; }
    }
}