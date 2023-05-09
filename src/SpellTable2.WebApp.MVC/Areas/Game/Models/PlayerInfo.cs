namespace SpellTable2.WebApp.MVC.Areas.Game.Models
{
    public class PlayerInfo
    {
        public Guid UserId { get; set; }

        public Guid PeerId { get; set; }

        public string PlayerName { get; set; }

        public int LifeTotal { get; set; }
    }
}