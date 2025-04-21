namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    /// <summary>
    /// Represents a request to update an existing product in the system.
    /// </summary>
    public class UpdateProductRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product to be updated.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the updated product name.
        /// Must be unique and not empty.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated short description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
