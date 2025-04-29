using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer
{
    /// <summary>
    /// Initializes a new instance of the UpdateCustomerRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Must be a valid GUID (not empty)
    /// - Name: Required, length between 3 and 100 characters
    /// - Email: Must be a valid email format
    /// - Status: Must be a valid CustomerStatus
    /// </remarks>
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(customer => customer.Id)
                .NotEmpty().WithMessage("Customer Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Customer Id must be a valid GUID.");

            RuleFor(customer => customer.Name)
                .NotEmpty().WithMessage("Customer name is required.")
                .Length(3, 100).WithMessage("Customer name must be between 3 and 100 characters.");

            RuleFor(customer => customer.Email)
                .SetValidator(new EmailValidator());

            RuleFor(customer => customer.Cpf)
                .SetValidator(new CpfValidator());

            RuleFor(customer => customer.Status)
                .IsInEnum().WithMessage("Customer status must be a valid value.");
        }
    }
}
