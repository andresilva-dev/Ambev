using Ambev.DeveloperEvaluation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer
{
    /// <summary>
    /// Response model for GetCustomer operation
    /// </summary>
    public class GetCustomerResult
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
        /// The customer's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The customer's cpf
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// The current status of the customer
        /// </summary>
        public CustomerStatus Status { get; set; }
    }
}
