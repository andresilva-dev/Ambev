namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Response model for retrieving sale details.
    /// </summary>
    public class GetSaleResponse
    {
        /// <summary>
        /// The unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The name of the customer who made the purchase.
        /// </summary>
        public string CustomerUsername { get; set; } = string.Empty;

        /// <summary>
        /// The branch where the sale occurred.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// The date and time the sale was created.
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
        /// List of items included in the sale.
        /// </summary>
        public List<GetSaleItemResponse> Items { get; set; } = new();
    }
}
