namespace BoardBrawl.Data.Application.Models
{
    public class Game
    {
        public int Id { get; set; }

        public Guid CreatedByUserId { get; set; }

        public string Name { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}