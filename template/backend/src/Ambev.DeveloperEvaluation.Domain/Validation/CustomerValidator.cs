using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Validator for the Customer entity.
    /// Applies business rules validation for customer properties.
    /// </summary>
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Email)
                .SetValidator(new EmailValidator());

            RuleFor(customer => customer.Name)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

            RuleFor(customer => customer.Cpf)
            .SetValidator(new CpfValidator());

            RuleFor(customer => customer.Status)
            .NotEqual(CustomerStatus.Unknown)
            .WithMessage("Customer status cannot be Unknown.");
        }
    }
}
