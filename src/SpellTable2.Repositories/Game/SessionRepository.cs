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
    }
}