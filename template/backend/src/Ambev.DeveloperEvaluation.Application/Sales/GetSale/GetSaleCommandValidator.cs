using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Validator for GetSaleCommand that defines validation rules for retrieving a sale.
    /// </summary>
    public class GetSaleCommandValidator : AbstractValidator<GetSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the GetSaleCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Id: Required and must not be an empty GUID.
        /// </remarks>
        public GetSaleCommandValidator()
        {
            RuleFor(sale => sale.Id)
                .NotEmpty().WithMessage("Sale ID is required.");
        }
    }
}
