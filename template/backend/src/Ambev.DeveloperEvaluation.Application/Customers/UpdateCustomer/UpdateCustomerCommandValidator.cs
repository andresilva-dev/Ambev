using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer
{
    /// <summary>
    /// Validator for UpdateCustomerCommand that defines validation rules for customer updating.
    /// </summary>
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateCustomerCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Id: Required and must not be empty
        /// - Name: Required, must not be empty
        /// - Cpf: Required and must be a valid CPF format
        /// - Email: Required and must be a valid email format
        /// - Status: Required
        /// </remarks>
        public UpdateCustomerCommandValidator()
        {
            RuleFor(customer => customer.Id)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(customer => customer.Name)
                .NotEmpty().WithMessage("Customer name is required.");

            RuleFor(customer => customer.Cpf)
                .SetValidator(new CpfValidator());

            RuleFor(customer => customer.Email)
                .SetValidator(new EmailValidator());

            RuleFor(customer => customer.Status)
                .IsInEnum().WithMessage("Customer status must be a valid value.");
        }
    }
}
