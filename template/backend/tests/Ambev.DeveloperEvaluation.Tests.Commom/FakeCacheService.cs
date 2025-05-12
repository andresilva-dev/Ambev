using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Tests.Commom
{
    public class FakeCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _store = new();

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var typeKey = typeof(T).FullName!;
            var json = JsonSerializer.Serialize(value);

            var typeStore = _store.GetOrAdd(typeKey, _ => new ConcurrentDictionary<string, string>());
            typeStore[key] = json;

            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            var typeKey = typeof(T).FullName!;
            if (_store.TryGetValue(typeKey, out var typeStore) &&
                typeStore.TryGetValue(key, out var json))
            {
                return Task.FromResult(JsonSerializer.Deserialize<T>(json));
            }

            return Task.FromResult<T?>(default);
        }

        public Task RemoveAsync<T>(string key)
        {
            var typeKey = typeof(T).FullName!;
            if (_store.TryGetValue(typeKey, out var typeStore))
            {
                typeStore.TryRemove(key, out _);
            }

            return Task.CompletedTask;
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            var typeKey = typeof(T).FullName!;
            if (_store.TryGetValue(typeKey, out var typeStore))
            {
                var result = typeStore.Values
                    .Select(json => JsonSerializer.Deserialize<T>(json))
                    .Where(x => x != null)
                    .ToList()!;
                return Task.FromResult(result);
            }

            return Task.FromResult(new List<T>());
        }

        public Task UpdateAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var typeKey = typeof(T).FullName!;
            if (_store.TryGetValue(typeKey, out var typeStore))
            {
                var json = JsonSerializer.Serialize(value);
                typeStore[key] = json;
            }

            return Task.CompletedTask;
        }
    }
}
