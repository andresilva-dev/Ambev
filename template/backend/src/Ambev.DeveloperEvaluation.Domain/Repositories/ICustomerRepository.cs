using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Customer entity operations
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Creates a new customer in the repository
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created customer</returns>
        Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a customer by their Email
        /// </summary>
        /// <param name="email">The Email to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a customer from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the customer was deleted, false if not found</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a customer by their Cpf
        /// </summary>
        /// <param name="cpf">The Cpf to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        Task<Customer?> GetByCpfAsync(string cpf, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all customers as an <see cref="IQueryable{Customer}"/> for further filtering and pagination.
        /// </summary>
        /// <returns>An <see cref="IQueryable{Customer}"/> representing all customer in the database.</returns>
        IQueryable<Customer> GetAllAsQueryable();

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated customer entity.</returns>
        Task<Customer> UpdateAsync(Customer existingCustomer, CancellationToken cancellationToken);
    }
}
