using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    /// <summary>
    /// Validator for GetSalesCommand
    /// </summary>
    public class GetSalesCommandValidator : AbstractValidator<GetSalesCommand>
    {
        public GetSalesCommandValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}
