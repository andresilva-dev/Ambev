namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer
{
    /// <summary>
    /// Request model for getting a customer by ID
    /// </summary>
    public class GetCustomerRequest
    {
        /// <summary>
        /// The unique identifier of the customer to retrieve
        /// </summary>
        public Guid Id { get; set; }
    }
}
