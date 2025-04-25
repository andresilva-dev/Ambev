using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale</returns>
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Retrieves a sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale if found, null otherwise</returns>
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Deletes a sale from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the sale was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Retrieves all sales as an <see cref="IQueryable{Sale}"/> for further filtering and pagination.
        /// </summary>
        /// <returns>An <see cref="IQueryable{Sale}"/> representing all sales in the database.</returns>
        public IQueryable<Sale> GetAllAsQueryable()
        {
            return _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Include(s => s.Customer)
                .AsQueryable();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Updates an existing sale in the repository.
        /// </summary>
        /// <param name="sale">The sale entity with updated values</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale entity</returns>
        public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }
    }
}
