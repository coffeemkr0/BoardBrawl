namespace BoardBrawl.Services.Game.Models
{
    public class CommanderDamage
    {
        public int CommanderOwnerPlayerId { get; set; }
        public string CommanderOwnerName { get; set; }
        public string CardId { get; set; }
        public int Damage { get; set; }
    }
}