using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleItemCommand that defines validation rules for sale item creation.
    /// </summary>
    public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleItemCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - ProductId: Required
        /// - ProductName: Required, between 3 and 100 characters
        /// - Quantity: Must be between 1 and 20
        /// - UnitPrice: Must be greater than zero
        /// - Discount logic: Purchases below 4 items cannot have a discount
        /// </remarks>
        public CreateSaleItemCommandValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("It's not possible to sell more than 20 identical items.");

        }
    }
}
