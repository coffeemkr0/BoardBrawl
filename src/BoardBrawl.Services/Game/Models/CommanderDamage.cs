namespace BoardBrawl.Services.Game.Models
{
    public class CommanderDamage
    {
        public int PlayerId { get; set; }
        public int CommanderOwnerPlayerId { get; set; }
        public string CardId { get; set; }
        public int Damage { get; set; }
    }
}