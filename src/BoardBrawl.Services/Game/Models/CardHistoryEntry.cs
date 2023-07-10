namespace BoardBrawl.Services.Game.Models
{
    public class CardHistoryEntry
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public int PlayerId { get; set; }

        public DateTime DateTimeAdded { get; set; }

        public string CardId { get; set; }
    }
}
