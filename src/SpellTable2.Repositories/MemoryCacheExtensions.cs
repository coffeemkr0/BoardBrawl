using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SpellTable2.Repositories;

namespace SpellTable2.Repositories
{
    internal static class MemoryCacheExtensions
    {
        public static T GetValueOrDefault<T>(this IMemoryCache cache, string key) where T : new()
        {
            var cacheValue = cache.Get(key);

            if (cacheValue == null) { return new T(); }

            if(cacheValue is T) return (T)cacheValue;

            if (cacheValue is string)
            {
                return JsonConvert.DeserializeObject<T>(cacheValue.ToString());
            }

            return (T)cacheValue;
        }

        public static void SerailizeObject(this IMemoryCache cache, string key, object value)
        {
            cache.Set(key, JsonConvert.SerializeObject(value));
        }
    }
}
