namespace BoardBrawl.Repositories.Game.Models
{
    public class GameInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? GameStart { get; set; }

        public DateTime? TurnStart { get; set; }

        public int? ActivePlayerId { get; set; }

        public List<PlayerInfo> Players{ get; }

        public GameInfo()
        {
            Players = new List<PlayerInfo>();
        }
    }
}