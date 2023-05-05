using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Repositories.Game
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
            var playerDictionary = _memoryCache.GetValueOrDefault<Dictionary<Guid, List<PlayerInfo>>>("Players");

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
            var games = GetGames();

            var game = games.FirstOrDefault(i => i.GameId == gameInfo.GameId);

            if (game != null)
            {
                games.Remove(game);
            }

            _memoryCache.Set("Games", games);
        }

        public GameInfo? GetGameInfo(Guid id)
        {
            return GetGames().FirstOrDefault(i => i.GameId == id);
        }

        public List<PlayerInfo> GetPlayers(Guid gameId)
        {
            var players = _memoryCache.GetValueOrDefault<Dictionary<Guid, List<PlayerInfo>>>("Players");

            return players[gameId];
        }

        private List<GameInfo> GetGames()
        {
            return _memoryCache.GetValueOrDefault<List<GameInfo>>("Games");
        }
    }
}