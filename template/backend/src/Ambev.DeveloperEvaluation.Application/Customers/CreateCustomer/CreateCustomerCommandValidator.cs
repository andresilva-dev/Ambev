using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    /// <summary>
    /// Validator for CreateCustomerCommand that defines validation rules for customer creation command.
    /// </summary>
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CreateCustomerCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Email: Must be in valid format (using EmailValidator)
        /// - Username: Required, must be between 3 and 100 characters
        /// - Status: Cannot be set to Unknown
        /// </remarks>
        public CreateCustomerCommandValidator()
        {
            RuleFor(customer => customer.Email).SetValidator(new EmailValidator());
            RuleFor(customer => customer.Cpf).SetValidator(new CpfValidator());
            RuleFor(customer => customer.Name).NotEmpty().Length(3, 100);
            RuleFor(customer => customer.Status).NotEqual(CustomerStatus.Unknown);
        }
    }
}
