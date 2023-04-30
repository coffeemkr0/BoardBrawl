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

        public string GetPlayerName()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("PlayerName");
        }

        public void SetPlayerName(string playerName)
        {
            _httpContextAccessor.HttpContext.Session.SetString("PlayerName", playerName);
        }

        private List<GameInfo> GetGames()
        {
            return _memoryCache.GetValueOrDefault<List<GameInfo>>("Games");
        }
    }
}