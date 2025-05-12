namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public static class SaleItemFactory
    {
        public static SaleItem Create(Guid productId, decimal unitPrice, int quantity, string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Name of product can't be null.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 items of the same product.");

            decimal discountPercentage = 0;

            if (quantity >= 10 && quantity <= 20)
            {
                discountPercentage = 0.20m;
            }
            else if (quantity >= 4)
            {
                discountPercentage = 0.10m;
            }

            var totalPrice = unitPrice * quantity * (1 - discountPercentage);

            return new SaleItem
            {
                ProductId = productId,
                UnitPrice = unitPrice,
                Quantity = quantity,
                DiscountPercentage = discountPercentage,
                Total = totalPrice               
            };
        }
    }
}
