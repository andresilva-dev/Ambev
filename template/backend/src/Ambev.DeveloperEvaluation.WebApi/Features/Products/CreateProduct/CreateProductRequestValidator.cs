using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct
{
    /// <summary>
    /// Initializes a new instance of the CreateProductRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Name: Required, length between 3 and 100 characters
    /// - Description: Optional, max length 250 characters
    /// - UnitPrice: Must be greater than 0
    /// </remarks>
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator() 
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
