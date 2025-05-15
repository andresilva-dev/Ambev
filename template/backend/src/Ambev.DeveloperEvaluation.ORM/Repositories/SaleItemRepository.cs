using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Repository for handling sale item-related database operations.
    /// </summary>
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleItemRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public SaleItemRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Retrieves the saleItem associated with a specific ID..
        /// </summary>
        /// <param name="id">The unique identifier of the sale item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The saleItem entity containing the sale item if found; otherwise, null.</returns>
        public async Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.SaleItems
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken)
        {
            _context.SaleItems.Update(saleItem);
            await _context.SaveChangesAsync(cancellationToken);
            return saleItem;
        }
    }
}
