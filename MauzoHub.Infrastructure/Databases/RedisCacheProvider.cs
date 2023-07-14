using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MauzoHub.Infrastructure.Databases
{
    public class RedisCacheProvider
    {
        private readonly IDistributedCache _cache;
        public RedisCacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value is not null)
            {
                return JsonSerializer.Deserialize<T>(value)!;
            }

            return default!;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
