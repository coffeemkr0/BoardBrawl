namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class CommanderDamage
    {
        public int PlayerId { get; set; }
        public int OwnerPlayerId { get; set; }
        public string OwnerPlayerName { get; set; }
        public string CardId { get; set; }
        public string CommanderName { get; set; }
        public int Damage { get; set; }
        public int DamagePercentage { get; set; }
    }
}