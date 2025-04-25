namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Represents a single item in a sale creation request.
    /// </summary>
    public class CreateSaleItemRequest
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
