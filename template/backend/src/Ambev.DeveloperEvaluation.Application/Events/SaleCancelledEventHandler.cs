using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale cancelled: {notification.Id}");
        return Task.CompletedTask;
    }
}
