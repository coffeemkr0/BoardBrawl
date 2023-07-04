namespace BoardBrawl.Data.Application.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string CreatedByUserId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsPublic { get; set; }

        public DateTime? GameStart { get; set; }

        public DateTime? TurnStart { get; set; }

        public int? ActivePlayerId { get; set; }

        public ICollection<Player> Players { get; set; }

        public ICollection<CommanderDamage> CommanderDamages { get; set; }
    }
}