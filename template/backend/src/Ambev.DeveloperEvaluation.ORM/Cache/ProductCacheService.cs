using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Cache
{
    public class ProductCacheService : IProductCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly string _prefix = "product:";

        public ProductCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public IDatabase Db => _db;

        public async Task<Product?> GetProductAsync(Guid id)
        {
            var data = await Db.StringGetAsync(_prefix + id);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Product>(data);
        }

        public async Task SetProductAsync(Product product)
        {
            var json = JsonSerializer.Serialize(product);
            await Db.StringSetAsync(_prefix + product.Id, json, TimeSpan.FromMinutes(10));
        }
    }
}
