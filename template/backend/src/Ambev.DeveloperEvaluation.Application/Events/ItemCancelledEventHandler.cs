using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Item cancelled: {notification.ItemId} of sale {notification.SaleId}");
        return Task.CompletedTask;
    }
}
