using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets the total value for this item after applying discount.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets the total value for this item after applying discount.
        /// </summary>
        public decimal TotalWithoutDiscounts => Quantity * UnitPrice;

        /// <summary>
        /// Gets or sets the foreign key reference to the parent sale.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Navigation property to the related sale.
        /// </summary>
        public Sale Sale { get; set; }

        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool Cancelled { get; private set; } = false;

        /// <summary>
        /// Cancels the saleItem and all its items.
        /// </summary>
        public void Cancel(bool cancel)
        {
            Cancelled = cancel;
        }

        /// <summary>
        /// Validates the sale entity against business rules.
        /// </summary>
        /// <returns>Validation result detail.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
