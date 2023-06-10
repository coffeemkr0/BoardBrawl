namespace BoardBrawl.Services.Game.Models
{
    public  class GameInfo
    {
        public int GameId { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
