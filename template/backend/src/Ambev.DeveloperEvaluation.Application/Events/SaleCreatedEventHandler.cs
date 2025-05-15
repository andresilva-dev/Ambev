using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale created: {notification.Id} in {notification.CreatedAt}");
        return Task.CompletedTask;
    }
}