using Microsoft.Extensions.Caching.Memory;

namespace SpellTable2.Repositories.Lobby
{
    public class MemoryRepository
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
    }
}