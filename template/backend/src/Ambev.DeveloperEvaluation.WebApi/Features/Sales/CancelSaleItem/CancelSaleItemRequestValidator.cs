using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelOrRestoreSaleItem
{
    /// <summary>
    /// Initializes a new instance of the CancelSaleItemRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleItemId: Required
    /// - Cancel: Must be specified (true or false)
    /// </remarks>
    public class CancelSaleItemRequestValidator : AbstractValidator<CancelSaleItemRequest>
    {
        public CancelSaleItemRequestValidator()
        {
            RuleFor(item => item.Id)
                .NotEmpty().WithMessage("Sale item ID is required.");

            RuleFor(item => item.Cancelled)
                .NotNull().WithMessage("Cancel flag must be specified.");
        }
    }
}
