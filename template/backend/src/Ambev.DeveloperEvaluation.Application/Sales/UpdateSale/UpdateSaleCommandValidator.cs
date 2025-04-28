using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleCommand that defines validation rules for updating a sale.
    /// </summary>
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - CustomerId: Required
        /// - Branch: Required, between 2 and 50 characters
        /// - Items: Must have at least one item
        /// </remarks>
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("ID is required.");

            RuleFor(sale => sale.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(sale => sale.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .Length(2, 50).WithMessage("Branch must be between 2 and 50 characters.");

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("At least one sale item is required.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new UpdateSaleItemCommandValidator());
        }
    }
}
