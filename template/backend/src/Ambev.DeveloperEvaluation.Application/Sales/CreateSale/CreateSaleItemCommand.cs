using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a sale item.
    /// </summary>
    /// <remarks>
    /// This command captures the necessary data for a sale item,
    /// including product ID, product name, quantity, and unit price.
    /// 
    /// It uses <see cref="CreateSaleItemCommandValidator"/> to ensure that the
    /// fields are valid and adhere to business rules.
    /// </remarks>
    public class CreateSaleItemCommand
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Validates the sale item against business rules.
        /// </summary>
        /// <returns>Validation result detail.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleItemCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
