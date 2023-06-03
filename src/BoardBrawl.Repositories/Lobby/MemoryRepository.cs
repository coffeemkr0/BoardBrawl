﻿using Microsoft.Extensions.Caching.Memory;
using BoardBrawl.Repositories.Lobby.Models;

namespace BoardBrawl.Repositories.Lobby
{
    public class MemoryRepository : IRepository
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void CreateGame(GameInfo gameInfo)
        {
            var games = GetGames();

            if (!games.Any(i => i.GameId == gameInfo.GameId))
            {
                games.Add(gameInfo);

                _memoryCache.SerailizeObject("Games", games);
            }
        }

        public void DeleteGame(Guid gameId)
        {
            var games = GetGames();

            var game = games.FirstOrDefault(i => i.GameId == gameId);

            if (game != null)
            {
                games.Remove(game);
            }

            _memoryCache.Set("Games", games);
        }

        public List<GameInfo> GetGames()
        {
            return _memoryCache.GetValueOrDefault<List<GameInfo>>("Games");
        }
    }
}