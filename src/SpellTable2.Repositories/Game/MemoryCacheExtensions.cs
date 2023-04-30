using Microsoft.Extensions.Caching.Memory;

namespace SpellTable2.Repositories.Game
{
    internal static class MemoryCacheExtensions
    {
        public static T GetValueOrDefault<T>(this IMemoryCache cache, string key) where T : new()
        {
            var cacheValue = cache.Get(key);

            if(cacheValue == null) { return new T(); }

            return (T)cacheValue;
        }
    }
}
