namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Represents an item within a sale.
    /// </summary>
    public class GetSaleItemResponse
    {
        /// <summary>
        /// The unique identifier of the product sold.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// The quantity of items sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product at the time of sale.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied (e.g., 0.10 = 10%).
        /// </summary>
        public decimal DiscountPercentage { get; set; }
    }
}
