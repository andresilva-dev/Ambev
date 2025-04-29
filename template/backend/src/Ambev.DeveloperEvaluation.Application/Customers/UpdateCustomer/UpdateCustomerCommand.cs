using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer
{
    /// <summary>
    /// Command for updating an existing customer.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for updating a customer, 
    /// including its identifier, name, CPF, email, and status. 
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns an <see cref="UpdateCustomerResult"/>.
    /// 
    /// The data provided in this command is validated using the 
    /// <see cref="UpdateCustomerCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and follow the required rules.
    /// </remarks>
    public class UpdateCustomerCommand : IRequest<UpdateCustomerResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the customer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the customer's name.
        /// Must not be empty or null.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's CPF.
        /// Must be a valid CPF format.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address.
        /// Must be a valid email format.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current status of the customer.
        /// Indicates whether the customer is active, inactive, or suspended.
        /// </summary>
        public CustomerStatus Status { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new UpdateCustomerCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
