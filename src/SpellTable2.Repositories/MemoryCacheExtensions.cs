using Microsoft.Extensions.Caching.Memory;
using SpellTable2.Repositories;

namespace SpellTable2.Repositories
{
    internal static class MemoryCacheExtensions
    {
        public static T GetValueOrDefault<T>(this IMemoryCache cache, string key) where T : new()
        {
            var cacheValue = cache.Get(key);

            if (cacheValue == null) { return new T(); }

            return (T)cacheValue;
        }
    }
}
