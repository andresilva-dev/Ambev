using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    /// <summary>
    /// Validator for CancelSaleItemCommand that defines validation rules for cancelling or restoring a sale item.
    /// </summary>
    public class CancelSaleItemCommandValidator : AbstractValidator<CancelSaleItemCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CancelSaleItemCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - ItemId: Required
        /// </remarks>
        public CancelSaleItemCommandValidator()
        {
            RuleFor(item => item.Id)
                .NotEmpty().WithMessage("Item ID is required.");
        }
    }
}
