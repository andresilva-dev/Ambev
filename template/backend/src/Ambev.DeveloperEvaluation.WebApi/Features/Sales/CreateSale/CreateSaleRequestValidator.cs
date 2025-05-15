using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId: Required
    /// - CustomerName: Required, length between 3 and 100 characters
    /// - Branch: Required, length between 2 and 50 characters
    /// - Items: Must contain at least one item and each item must pass individual validation
    /// </remarks>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(sale => sale.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(sale => sale.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .Length(2, 50).WithMessage("Branch name must be between 2 and 50 characters.");

            RuleFor(sale => sale.Items)
                .NotNull().WithMessage("At least one sale item is required.")
                .Must(items => items.Any()).WithMessage("The sale must contain at least one item.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}
