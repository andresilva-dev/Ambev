using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface IProductCacheService
    {
        Task<Product?> GetProductAsync(Guid id);
        Task SetProductAsync(Product product);
    }
}
