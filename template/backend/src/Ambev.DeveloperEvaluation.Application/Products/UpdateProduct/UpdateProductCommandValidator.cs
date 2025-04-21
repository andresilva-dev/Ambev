using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    /// <summary>
    /// Validator for UpdateProductCommand that defines validation rules for product updating.
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateProductCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Id: Required and must not be empty
        /// - Name: Required, must be between 3 and 100 characters
        /// - Description: Optional, maximum length of 250 characters
        /// - UnitPrice: Must be greater than zero
        /// </remarks>
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.Id)
                .NotEmpty().WithMessage("Product ID is required.");

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
