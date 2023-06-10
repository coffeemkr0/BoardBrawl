namespace BoardBrawl.Repositories.Lobby.Models
{
    public class GameInfo
    {
        public int Id { get; set; }

        public Guid CreatedByUserId { get; set; }

        public string Name { get; set; }

        public int PlayerCount { get; set; }
    }
}