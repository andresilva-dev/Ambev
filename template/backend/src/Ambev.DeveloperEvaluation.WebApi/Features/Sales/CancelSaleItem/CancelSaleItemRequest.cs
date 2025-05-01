namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelOrRestoreSaleItem
{
    /// <summary>
    /// Represents the request to cancel or restore a sale item.
    /// </summary>
    public class CancelSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the ID of the sale item to be cancelled or restored.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item should be cancelled (true) or restored (false).
        /// </summary>
        public bool Cancelled { get; set; }
    }
}
