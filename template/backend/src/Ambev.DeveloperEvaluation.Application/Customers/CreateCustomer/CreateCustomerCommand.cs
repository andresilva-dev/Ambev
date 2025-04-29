using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    /// <summary>
    /// Command for creating a new customer.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for creating a customer, 
    /// including CPF, name, email, status, and created/updated dates. 
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="CreateCustomerResult"/>.
    /// 
    /// The data provided in this command is validated using the 
    /// <see cref="CreateCustomerCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and follow the required rules.
    /// </remarks>
    public class CreateCustomerCommand : IRequest<CreateCustomerResult>
    {
        /// <summary>
        /// Gets or sets the CPF of the customer to be created.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the customer.
        /// </summary>
        public CustomerStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the last updated date of the customer.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new CreateCustomerCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
