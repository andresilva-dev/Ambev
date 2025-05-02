using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : INotification
    {
        public Guid Id { get; }

        public SaleCancelledEvent(Guid saleId)
        {
            Id = saleId;
        }
    }
}
