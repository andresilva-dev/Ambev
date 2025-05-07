using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Product entity operations
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new product in the repository
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created product</returns>
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a product by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The product if found, null otherwise</returns>
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a product from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the product was deleted, false if not found</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all products as an <see cref="IQueryable{Product}"/> for further filtering and pagination.
        /// </summary>
        /// <returns>An <see cref="IQueryable{Product}"/> representing all products in the database.</returns>
        IEnumerable<Product> GetAll();

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated product entity.</returns>
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
    }
}
