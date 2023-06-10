namespace BoardBrawl.Data.Application.Models
{
    public class Player
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public Guid PeerId { get; set; }

        public string Name { get; set; }

        public int GameId { get; set; }

        public int LifeTotal { get; set; }

        public int CommanderDamage { get; set; }

        public int InfectDamage { get; set; }

        public Game Game { get; set; }
    }
}