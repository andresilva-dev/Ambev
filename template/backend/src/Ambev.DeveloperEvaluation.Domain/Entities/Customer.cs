using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a customer in the system.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class Customer : BaseEntity
    {
        /// <summary>
        /// Gets the customer's CPF.
        /// Must be a valid CPF format.
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Gets the customer's full name.
        /// Must not be null or empty.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the customer's email address.
        /// Must be a valid email format.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets the customer's current status.
        /// Indicates whether the customer is active, inactive, or suspended in the system.
        /// </summary>
        public CustomerStatus Status { get; set; }

        /// <summary>
        /// Gets the date and time when the customer was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets the date and time of the last update to the customer's information.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the Customer class.
        /// </summary>
        public Customer()
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Performs validation of the customer entity using the CustomerValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">CPF format</list>
        /// <list type="bullet">Name format and length</list>
        /// <list type="bullet">Email format</list>
        /// </remarks>
        public ValidationResultDetail Validate()
        {
            var validator = new CustomerValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }

        /// <summary>
        /// Activates the customer account.
        /// Changes the customer's status to Active.
        /// </summary>
        public void Activate()
        {
            Status = CustomerStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates the customer account.
        /// Changes the customer's status to Inactive.
        /// </summary>
        public void Deactivate()
        {
            Status = CustomerStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Suspends the customer account.
        /// Changes the customer's status to Suspended.
        /// </summary>
        public void Suspend()
        {
            Status = CustomerStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
