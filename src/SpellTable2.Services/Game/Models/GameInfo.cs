namespace SpellTable2.Services.Game.Models
{
    public  class GameInfo
    {
        public Guid GameId { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
