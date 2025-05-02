using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent : INotification
    {
        public Guid Id { get; }
        public DateTime UpdatedAt { get; }

        public SaleModifiedEvent(Guid saleId, DateTime updatedAt)
        {
            Id = saleId;
            UpdatedAt = updatedAt;
        }
    }
}
