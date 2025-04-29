using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer
{
    /// <summary>
    /// Validator for CreateCustomerRequest that defines validation rules for customer creation.
    /// </summary>
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CreateCustomerRequestValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Name: Required, length between 3 and 100 characters
        /// - Cpf: Must be a valid CPF format (using CpfValidator)
        /// - Email: Must be valid format (using EmailValidator)
        /// - Status: Cannot be Unknown
        /// </remarks>
        public CreateCustomerRequestValidator()
        {
            RuleFor(customer => customer.Name)
                .NotEmpty()
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(customer => customer.Cpf)
                .SetValidator(new CpfValidator());

            RuleFor(customer => customer.Email)
                .SetValidator(new EmailValidator());

            RuleFor(customer => customer.Status)
                .NotEqual(CustomerStatus.Unknown)
                .WithMessage("Customer status cannot be Unknown.");
        }
    }
}
