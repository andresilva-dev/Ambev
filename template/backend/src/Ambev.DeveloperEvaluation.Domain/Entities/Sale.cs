using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction including customer and product details.
    /// This entity includes domain logic and validation.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique sale number.
        /// </summary>
        public string SaleNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8);

        /// <summary>
        /// Gets or sets the date the sale was made.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the branch where the sale occurred.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool Cancelled { get; private set; } = false;

        /// <summary>
        /// The list of items sold in this sale.
        /// </summary>
        public List<SaleItem> Items { get; set; } = new();

        /// <summary>
        /// The list of items sold in this sale.
        /// </summary>
        public List<SaleItem> ItemsNotCancelled => Items.Where(i => i.Cancelled == false).ToList();

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal Total => ItemsNotCancelled.Sum(i => i.Total);

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal TotalWithoutDiscounts => ItemsNotCancelled.Sum(i => i.TotalWithoutDiscounts);

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal TotalDiscountsPercentage => (1 - Math.Round((Total / TotalWithoutDiscounts), 2, MidpointRounding.AwayFromZero)) * 100; 

        public User Customer { get; set; }

        /// <summary>
        /// Initializes a new instance of the Sale class.
        /// </summary>
        public Sale() => Date = DateTime.UtcNow;

        /// <summary>
        /// Adds an item to the sale with business rule validation.
        /// </summary>
        public void AddItem(Guid productId, int quantity, decimal unitPrice, string productName)
        {
            var item = SaleItemFactory.Create(productId, unitPrice, quantity, productName);
            Items.Add(item);
        }

        /// <summary>
        /// Cancels the sale and all its items.
        /// </summary>
        public void Cancel(bool cancel)
        {
            Cancelled = cancel;

            foreach (var item in Items)
            {
                item.Cancel(cancel);
            }
        }

        /// <summary>
        /// Validates the sale entity against business rules.
        /// </summary>
        /// <returns>Validation result detail.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
