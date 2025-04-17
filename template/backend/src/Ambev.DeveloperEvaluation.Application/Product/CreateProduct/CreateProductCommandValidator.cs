using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Product.CreateProduct
{
    /// <summary>
    /// Validator for CreateProductCommand that defines validation rules for product creation.
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Name: Required, must be between 3 and 100 characters
        /// - Description: Optional, maximum length of 250 characters
        /// - UnitPrice: Must be greater than zero
        /// </remarks>
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(product => product.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(product => product.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
