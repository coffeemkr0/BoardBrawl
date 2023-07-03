﻿namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Guid PeerId { get; set; }
        public string PlayerName { get; set; }
        public int LifeTotal { get; set; }
        public int InfectDamage { get; set; }
        public string? Commander1Id { get; set; }
        public CardInfo? Commander1 { get; set; }
        public string? Commander2Id { get; set; }
        public CardInfo? Commander2 { get; set; }
        public List<Colors> CommanderColors { get; }
        public bool IsSelf { get; set; }
        public List<CommanderDamage> CommanderDamages { get; }

        public PlayerInfo()
        {
            CommanderColors = new List<Colors>();
            CommanderDamages = new List<CommanderDamage>();
        }
    }
}