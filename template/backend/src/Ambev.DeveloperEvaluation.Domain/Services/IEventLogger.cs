namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface IEventLogger
    {
        Task LogAsync(string eventType, string id, CancellationToken cancellationToken = default);
    }
}
