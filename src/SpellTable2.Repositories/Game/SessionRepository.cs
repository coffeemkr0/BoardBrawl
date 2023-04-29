using Microsoft.AspNetCore.Http;

namespace SpellTable2.Repositories.Game
{
    public class SessionRepository : IRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetPlayerName()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("PlayerName");
        }

        public void SetPlayerName(string playerName)
        {
            _httpContextAccessor.HttpContext.Session.SetString("PlayerName", playerName);
        }
    }
}