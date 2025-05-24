using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly IEventLogger _eventLogger;

    public SaleCancelledEventHandler(IEventLogger eventLogger)
    {
        _eventLogger = eventLogger;
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale cancelled: {notification.Id}");

        await _eventLogger.LogAsync(
            eventType: nameof(SaleCancelledEventHandler),
            id: notification.Id.ToString(),
            cancellationToken);
    }
}
