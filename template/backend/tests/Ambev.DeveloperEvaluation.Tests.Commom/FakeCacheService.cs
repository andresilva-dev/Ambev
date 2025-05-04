using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Tests.Commom
{
    public class FakeCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, string> _store = new();

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            _store[key] = json;
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_store.TryGetValue(key, out var value)
                ? JsonSerializer.Deserialize<T>(value)
                : default);
        }

        public Task RemoveAsync(string key)
        {
            _store.TryRemove(key, out _);
            return Task.CompletedTask;
        }
    }
}
