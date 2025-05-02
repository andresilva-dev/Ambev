using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : INotification
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public SaleCreatedEvent(Guid saleId, DateTime createdAt)
        {
            Id = saleId;
            CreatedAt = createdAt;
        }
    }
}
