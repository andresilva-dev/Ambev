using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    /// <summary>
    /// Validator for GetProductsCommand
    /// </summary>
    public class GetProductsCommandValidator : AbstractValidator<GetProductsCommand>
    {
        public GetProductsCommandValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}
