namespace BoardBrawl.Data.Application.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public Guid PeerId { get; set; }

        public string Name { get; set; }

        public int GameId { get; set; }

        public int? FocusedPlayerId { get; set; }

        public int LifeTotal { get; set; }

        public int InfectCount { get; set; }

        public string? Commander1Id { get; set; }

        public string? Commander2Id { get; set; }

        public int TurnOrder { get; set; }

        public Game Game { get; set; }
    }
}