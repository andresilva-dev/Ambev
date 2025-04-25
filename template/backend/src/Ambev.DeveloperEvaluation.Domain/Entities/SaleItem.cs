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
        /// Gets or sets the discount percentage applied (e.g., 0.10 = 10%).
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets the total value for this item after applying discount.
        /// </summary>
        public decimal Total => Quantity * UnitPrice * (1 - DiscountPercentage);

        /// <summary>
        /// Gets or sets the foreign key reference to the parent sale.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Navigation property to the related sale.
        /// </summary>
        public Sale Sale { get; set; }

        /// <summary>
        /// Navigation property to the related product.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleItem"/> class.
        /// Applies basic validation and discount policy.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="quantity">The quantity sold.</param>
        /// <param name="unitPrice">The unit price of the product.</param>
        /// <param name="discountPercentage">The discount to apply.</param>
        /// <exception cref="ArgumentException">Thrown if quantity is invalid.</exception>
        public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discountPercentage)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 items of the same product.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountPercentage = discountPercentage;
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

        /// <summary>
        /// Default constructor for EF Core.
        /// </summary>
        public SaleItem() { }
    }
}
