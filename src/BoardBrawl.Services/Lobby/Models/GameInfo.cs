﻿namespace BoardBrawl.Services.Lobby.Models
{
    public class GameInfo
    {
        public int Id { get; set; }

        public Guid CreatedByUserId { get; set; }

        public bool IsPublic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PlayerCount { get; set; }
    }
}