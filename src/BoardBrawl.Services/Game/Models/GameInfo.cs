namespace BoardBrawl.Services.Game.Models
{
    public  class GameInfo
    {
        public int Id { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ActivePlayerId { get; set; }
    }
}
