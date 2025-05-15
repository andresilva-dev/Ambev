using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validator for <see cref="UpdateSaleRequest"/>.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId: Required
    /// - Branch: Required, length between 2 and 100 characters
    /// - Items: Must not be empty, each item validated by UpdateSaleItemRequestValidator
    /// </remarks>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .Length(2, 100).WithMessage("Branch must be between 2 and 100 characters long.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item must be included in the sale.");

            RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemRequestValidator());
        }
    }
}
