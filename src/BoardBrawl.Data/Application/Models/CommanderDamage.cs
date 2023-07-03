using BoardBrawl.Data.Application.Models;

namespace BoardBrawl.Repositories.Game.Models
{
    public class CommanderDamage
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string CommanderName { get; set; }
        public string CardId { get; set; }
        public int Damage { get; set; }
        public Player Player { get; set; }
    }
}