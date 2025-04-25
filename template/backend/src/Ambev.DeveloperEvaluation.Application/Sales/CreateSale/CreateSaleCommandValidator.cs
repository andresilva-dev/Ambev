using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleCommand that defines validation rules for creating a sale.
    /// </summary>
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - CustomerId: Required
        /// - Branch: Required, between 2 and 50 characters
        /// - Items: Must have at least one item
        /// </remarks>
        public CreateSaleCommandValidator()
        {
            RuleFor(sale => sale.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(sale => sale.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .Length(2, 50).WithMessage("Branch must be between 2 and 50 characters.");

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("At least one sale item is required.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new CreateSaleItemCommandValidator());
        }
    }
}
