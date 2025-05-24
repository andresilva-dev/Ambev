using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly IEventLogger _eventLogger;

    public SaleModifiedEventHandler(IEventLogger eventLogger)
    {
        _eventLogger = eventLogger;
    }

    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale modified: {notification.Id}");
        
        await _eventLogger.LogAsync(
            eventType: nameof(SaleModifiedEventHandler),
            id: notification.Id.ToString(),
            cancellationToken);
    }
}
