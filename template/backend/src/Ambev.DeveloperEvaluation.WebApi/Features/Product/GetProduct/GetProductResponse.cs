namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct
{
    public class GetProductResponse
    {
        /// <summary>
        /// The unique identifier of the created product
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// Must be unique and not empty.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a short description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
