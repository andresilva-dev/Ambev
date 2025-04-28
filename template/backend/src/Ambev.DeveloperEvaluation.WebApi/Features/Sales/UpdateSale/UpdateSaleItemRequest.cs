namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Represents a single item in a sale update request.
    /// </summary>
    public class UpdateSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the ID of the product being sold.
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product being sold.
        /// </summary>
        public int Quantity { get; set; }
    }
}
