namespace BoardBrawl.Repositories.Game.Models
{
    public class CommanderDamage
    {
        public int PlayerId { get; set; }
        public int OwnerPlayerId { get; set; }
        public string CardId { get; set; }
        public int Damage { get; set; }
    }
}