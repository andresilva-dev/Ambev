namespace Ambev.DeveloperEvaluation.Application.Product.GetProduct
{
    public class GetProductResult
    {
        /// <summary>
        /// The unique identifier of the product.
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
