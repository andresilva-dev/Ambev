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
        public bool Cancelled { get; set; } = false;

        /// <summary>
        /// The list of items sold in this sale.
        /// </summary>
        public List<SaleItem> Items { get; set; } = new();

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal Total => Items.Sum(i => i.Total);

        /// <summary>
        /// Total value of the sale, considering discounts.
        /// </summary>
        public decimal TotalWithoutDiscounts => Items.Sum(i => i.TotalWithoutDiscounts);

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
        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            decimal discount = 0;
            if (quantity >= 10) discount = 20m;
            else if (quantity >= 4) discount = 10m;

            Items.Add(new SaleItem(productId, productName, quantity, unitPrice, discount));
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
