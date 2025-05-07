using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.IoC.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var redisKey = $"{typeof(T).Name}:{key}";
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(redisKey, json, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var redisKey = $"{typeof(T).Name}:{key}";
            var value = await _db.StringGetAsync(redisKey);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
        }

        public async Task RemoveAsync<T>(string key)
        {
            var redisKey = $"{typeof(T).Name}:{key}";
            await _db.KeyDeleteAsync(redisKey);
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            var pattern = $"{typeof(T).Name}:*";
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern);

            var result = new List<T>();

            foreach (var key in keys)
            {
                var value = await _db.StringGetAsync(key);
                if (!value.IsNullOrEmpty)
                {
                    var item = JsonSerializer.Deserialize<T>(value!);
                    if (item is not null)
                        result.Add(item);
                }
            }

            return result;
        }
    }
}
