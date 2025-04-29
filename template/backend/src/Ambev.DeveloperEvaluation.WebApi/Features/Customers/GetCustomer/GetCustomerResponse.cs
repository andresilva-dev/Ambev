using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer
{
    /// <summary>
    /// API response model for GetCustomer operation
    /// </summary>
    public class GetCustomerResponse
    {
        /// <summary>
        /// The unique identifier of the customer
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The customer's full name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The customer's CPF number
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// The customer's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The customer's status 
        /// </summary>
        public CustomerStatus Status { get; set; }

    }
}
