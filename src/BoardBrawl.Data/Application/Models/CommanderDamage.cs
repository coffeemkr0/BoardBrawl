namespace BoardBrawl.Data.Application.Models
{
    public class CommanderDamage
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int OwnerPlayerId { get; set; }
        public string CardId { get; set; }
        public int Damage { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}