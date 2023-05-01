namespace SpellTable2.WebApp.MVC.Areas.Game.Models
{
    public class PlayerList
    {
        public List<PlayerInfo> Players { get; }

        public PlayerList()
        {
            Players= new List<PlayerInfo>();
        }
    }
}