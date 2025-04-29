using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer
{
    /// <summary>
    /// Represents a request to create a new customer in the system.
    /// </summary>
    public class CreateCustomerRequest
    {
        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's CPF. Must be a valid CPF format.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address. Must be a valid email format.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initial status of the customer.
        /// </summary>
        public CustomerStatus Status { get; set; }
    }
}
