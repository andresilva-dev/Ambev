using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    /// <summary>
    /// Command for cancelling or uncancelling a sale item.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for cancelling or restoring 
    /// a sale item, based on a boolean flag.
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="CancelSaleItemResult"/>.
    ///
    /// The data provided in this command is validated using the 
    /// <see cref="CancelSaleItemCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly populated.
    /// </remarks>
    public class CancelSaleItemCommand : IRequest<CancelSaleItemResult>
    {
        /// <summary>
        /// Gets or sets the ID of the sale item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item should be cancelled or restored.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Validates the command against business rules.
        /// </summary>
        /// <returns>Validation result detail.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new CancelSaleItemCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
