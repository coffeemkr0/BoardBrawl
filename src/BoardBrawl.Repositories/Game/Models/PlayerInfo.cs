namespace BoardBrawl.Repositories.Game.Models
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int? FocusedPlayerId { get; set; }
        public Guid PeerId { get; set; }
        public int LifeTotal { get; set; }
        public int InfectCount { get; set; }
        public string Commander1Id { get; set; }
        public string Commander2Id { get; set; }
    }
}