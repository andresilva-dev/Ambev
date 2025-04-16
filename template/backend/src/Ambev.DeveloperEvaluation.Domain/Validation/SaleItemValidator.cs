using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Provides validation rules for the <see cref="SaleItem"/> entity.
    /// </summary>
    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        public SaleItemValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(item => item.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 units of the same product.");

            RuleFor(item => item.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(item => item.DiscountPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
                .LessThanOrEqualTo(0.20m).WithMessage("Maximum allowed discount is 20%.")
                .Must((item, discount) =>
                {
                    if (item.Quantity >= 10 && discount == 0.20m) return true;
                    if (item.Quantity >= 4 && item.Quantity < 10 && discount == 0.10m) return true;
                    if (item.Quantity < 4 && discount == 0.00m) return true;
                    return false;
                }).WithMessage("The discount is not valid for the given quantity.");
        }
    }
}
