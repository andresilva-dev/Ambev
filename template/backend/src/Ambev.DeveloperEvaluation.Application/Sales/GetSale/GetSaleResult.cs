    namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents the result of retrieving a sale by its ID.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// The unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the customer ID related to the sale.
        /// </summary>
        public Guid CustomerId { get; set; }
        
        /// <summary>
        /// Gets or sets the customer Name related to the sale.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the branch where the sale was made.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date the sale was made.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal TotalDiscountsPercentage { get; set; }

        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Gets or sets the list of sale items.
        /// </summary>
        public List<GetSaleItemResult> Items { get; set; } = new();
    }
}
