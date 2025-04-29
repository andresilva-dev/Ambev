namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    /// <summary>
    /// Represents the response returned after successfully cancelling or restoring a sale item.
    /// </summary>
    /// <remarks>
    /// This response contains the unique identifier of the sale item that was updated,
    /// which can be used for subsequent operations or reference.
    /// </remarks>
    public class CancelSaleItemResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the updated sale item.
        /// </summary>
        /// <value>A GUID that uniquely identifies the sale item in the system.</value>
        public Guid Id { get; set; }
    }
}
