using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer
{
    /// <summary>
    /// Represents a request to update an existing customer in the system.
    /// </summary>
    public class UpdateCustomerRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the customer to be updated.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the updated full name of the customer.
        /// Must not be empty or null.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated email address of the customer.
        /// Must be a valid email format.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated CPF of the customer.
        /// Must be a valid CPF format.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the customer.
        /// </summary>
        public CustomerStatus Status { get; set; }
    }
}
