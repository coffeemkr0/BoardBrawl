using Microsoft.Extensions.Caching.Memory;
using SpellTable2.Repositories.Lobby.Models;

namespace SpellTable2.Repositories.Lobby
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
                _memoryCache.Set("Games", games);
            }
        }

        public List<GameInfo> GetGames()
        {
            return _memoryCache.GetValueOrDefault<List<GameInfo>>("Games");
        }
    }
}