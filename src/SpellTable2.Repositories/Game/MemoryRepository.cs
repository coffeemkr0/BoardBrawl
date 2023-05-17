﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Repositories.Game
{
    public class MemoryRepository : IRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public MemoryRepository(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        public void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo)
        {
            var playerDictionary = GetPlayerCollection();

            if (!playerDictionary.ContainsKey(gameId))
            {
                playerDictionary.Add(gameId, new List<PlayerInfo>());
            }

            if (!playerDictionary[gameId].Any(i=>i.UserId== playerInfo.UserId))
            {
                playerDictionary[gameId].Add(playerInfo);
            }

            _memoryCache.Set("Players", playerDictionary);
        }

        public void CloseGame(GameInfo gameInfo)
        {
            var games = GetGameCollection();

            var game = games.FirstOrDefault(i => i.GameId == gameInfo.GameId);

            if (game != null)
            {
                games.Remove(game);
            }

            _memoryCache.Set("Games", games);
        }

        public void DecreaseLifeTotal(Guid gameId, Guid userId, int amount)
        {
            var player = GetPlayer(gameId, userId);
            player.LifeTotal -= amount;
        }

        public GameInfo? GetGameInfo(Guid id)
        {
            return GetGameCollection().FirstOrDefault(i => i.GameId == id);
        }

        public List<PlayerInfo> GetPlayers(Guid gameId)
        {
            var players = GetPlayerCollection();
            return players[gameId];
        }

        public void IncreaseLifeTotal(Guid gameId, Guid userId, int amount)
        {
            var player = GetPlayer(gameId, userId);
            player.LifeTotal += amount;
        }

        public void UpdatePeerId(Guid gameId, Guid userId, Guid peerId)
        {
            var player = GetPlayer(gameId, userId);
            player.PeerId = peerId;
        }

        private List<GameInfo> GetGameCollection()
        {
            return _memoryCache.GetValueOrDefault<List<GameInfo>>("Games");
        }

        private Dictionary<Guid, List<PlayerInfo>> GetPlayerCollection()
        {
            return _memoryCache.GetValueOrDefault<Dictionary<Guid, List<PlayerInfo>>>("Players");
        }

        private PlayerInfo GetPlayer(Guid gameId, Guid userId)
        {
            return GetPlayerCollection()[gameId].First(i => i.UserId == userId);
        }

        public void ClearPlayers()
        {
            GetPlayerCollection().Clear();
        }
    }
}