namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct
{
    /// <summary>
    /// Request model for getting a product by ID
    /// </summary>
    public class GetProductRequest
    {
        /// <summary>
        /// The unique identifier of the product to retrieve
        /// </summary>
        public Guid Id { get; set; }
    }
}
