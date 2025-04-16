using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(s => s.CustomerId).NotEmpty();
            RuleFor(s => s.CustomerName).NotEmpty();
            RuleFor(s => s.Branch).NotEmpty();
            RuleFor(s => s.Items).NotEmpty();
        }
    }
}
