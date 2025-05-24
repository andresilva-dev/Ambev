using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly IEventLogger _eventLogger;

    public SaleCreatedEventHandler(IEventLogger eventLogger)
    {
        _eventLogger = eventLogger;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale created: {notification.Id} in {notification.CreatedAt}");

        await _eventLogger.LogAsync(
            eventType: nameof(SaleCreatedEvent),
             id: notification.Id.ToString(),
            cancellationToken);

    }
}