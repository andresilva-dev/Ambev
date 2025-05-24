using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly IEventLogger _eventLogger;

    public ItemCancelledEventHandler(IEventLogger eventLogger)
    {
        _eventLogger = eventLogger;
    }

    public async Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Item cancelled: {notification.ItemId} of sale {notification.SaleId}");
        
        await _eventLogger.LogAsync(
            eventType: nameof(ItemCancelledEvent),
            id: notification.ItemId.ToString(),
            cancellationToken);
    }
}
