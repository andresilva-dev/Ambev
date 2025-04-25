using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Sale entity operations
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale.</returns>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a sale from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the sale was deleted, false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all sales as an <see cref="IQueryable{Sale}"/> for further filtering and pagination.
        /// </summary>
        /// <returns>An <see cref="IQueryable{Sale}"/> representing all sales in the database.</returns>
        IQueryable<Sale> GetAllAsQueryable();

        /// <summary>
        /// Updates an existing sale in the database.
        /// </summary>
        /// <param name="sale">The sale entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale entity.</returns>
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    }
}
