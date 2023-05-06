namespace SpellTable2.Services.Game.Models
{
    public class PlayerInfo
    {
        public Guid UserId { get; set; }

        public string PlayerName { get; set; }

        public int LifeTotal { get; set; }
    }
}
