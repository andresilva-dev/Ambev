namespace Ambev.DeveloperEvaluation.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync<T>(string key);
        Task<List<T>> GetAllAsync<T>();
    }
}
