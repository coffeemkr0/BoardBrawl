namespace BoardBrawl.Data.Application.Models
{
    public class Player
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}