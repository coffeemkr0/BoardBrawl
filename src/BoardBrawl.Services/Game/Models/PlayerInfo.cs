using System.Security.Cryptography;

namespace BoardBrawl.Services.Game.Models
{
    public class PlayerInfo
    {
        public Guid UserId { get; set; }

        public Guid PeerId { get; set; }

        public string PlayerName { get; set; }

        public int LifeTotal { get; set; }
        public int CommanderDamage { get; set; }
        public int InfectDamage { get; set; }



    }
}
