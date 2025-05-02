using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale modified: {notification.Id}");
        return Task.CompletedTask;
    }
}
