namespace BoardBrawl.Repositories.Game.Models
{
    public class PlayerInfo
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public Guid PeerId { get; set; }

        public int LifeTotal { get; set; }

        public int CommanderDamage { get; set; }

        public int InfectDamage { get; set; }
    }
}