namespace BoardBrawl.Repositories.Models
{
    public class PlayerInfo
    {
        public Guid UserId { get; set; }

        public Guid PeerId { get; set; }

        public string PlayerName { get; set; }

        public int LifeTotal { get; set; }

        public int InfectDamage { get; set; }

        public int CommanderDamage { get; set; }

    }
}