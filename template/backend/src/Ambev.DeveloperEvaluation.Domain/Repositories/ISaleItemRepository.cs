using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for SaleItem entity operations
    /// </summary>
    public interface ISaleItemRepository
    {
        /// <summary>
        /// Retrieves a saleItem by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the saleItem.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>saleItem sale if found, null otherwise.</returns>
        Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken);
    }
}
