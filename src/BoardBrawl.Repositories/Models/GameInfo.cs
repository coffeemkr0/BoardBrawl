﻿namespace BoardBrawl.Repositories.Models
{
    public class GameInfo
    {
        public Guid GameId { get; set; }

        public Guid CreatedByUserId { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PlayerInfo> Players { get; }

        public GameInfo()
        {
            Players = new List<PlayerInfo>();
        }
    }
}