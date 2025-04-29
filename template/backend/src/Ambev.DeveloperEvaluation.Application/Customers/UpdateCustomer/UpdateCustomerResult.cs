namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer
{
    /// <summary>
    /// Represents the response returned after successfully updating a customer.
    /// </summary>
    /// <remarks>
    /// This response contains the unique identifier of the updated customer,
    /// which can be used for confirmation or subsequent operations.
    /// </remarks>
    public class UpdateCustomerResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the updated customer.
        /// </summary>
        /// <value>A GUID that uniquely identifies the updated customer in the system.</value>
        public Guid Id { get; set; }
    }
}
