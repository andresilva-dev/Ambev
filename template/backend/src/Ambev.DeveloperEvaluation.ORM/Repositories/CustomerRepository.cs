using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of ICustomerRepository using Entity Framework Core
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of CustomerRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public CustomerRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new customer in the database
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created customer</returns>
        public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Deletes a customer from the database
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the customer was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var customer = await GetByIdAsync(id, cancellationToken);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Retrieves a customer by their Email
        /// </summary>
        /// <param name="email">The Email to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }

        /// <summary>
        /// Retrieves a customer by their Cpf
        /// </summary>
        /// <param name="cpf">The Cpf to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer if found, null otherwise</returns>
        public async Task<Customer?> GetByCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);
        }

        /// <summary>
        /// Retrieves all customer as an <see cref="IQueryable{Customer}"/> for further filtering and pagination.
        /// </summary>
        /// <returns>An <see cref="IQueryable{Customer}"/> representing all customers in the database.</returns>
        public IQueryable<Customer> GetAllAsQueryable()
        {
            return _context.Customers.AsNoTracking();
        }

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated customer entity.</returns>
        public async Task<Customer> UpdateAsync(Customer existingCustomer, CancellationToken cancellationToken)
        {
            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync(cancellationToken);

            return existingCustomer;
        }
    }
}
