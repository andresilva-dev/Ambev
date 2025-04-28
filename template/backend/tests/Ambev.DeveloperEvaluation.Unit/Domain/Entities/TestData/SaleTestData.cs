using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data for Sale and SaleItem entities.
    /// </summary>
    public static class SaleTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .RuleFor(si => si.ProductId, f => Guid.NewGuid())
            .RuleFor(si => si.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(si => si.UnitPrice, f => decimal.Parse(f.Commerce.Price(1, 1000)))
            .RuleFor(si => si.DiscountPercentage, (f, si) =>
            {
                if (si.Quantity >= 10) return 20m;
                if (si.Quantity >= 4) return 10m;
                return 0m;
            });

        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .RuleFor(s => s.Date, f => DateTime.UtcNow)
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => SaleItemFaker.Generate(f.Random.Int(1, 5)));

        /// <summary>
        /// Generates a valid Sale entity with random valid SaleItems.
        /// </summary>
        /// <returns>A valid Sale entity.</returns>
        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

        /// <summary>
        /// Generates a valid SaleItem entity.
        /// </summary>
        /// <returns>A valid SaleItem entity.</returns>
        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }

        /// <summary>
        /// Generates a valid SaleItem quantity (1 to 20).
        /// </summary>
        /// <returns>A valid quantity.</returns>
        public static int GenerateValidQuantity()
        {
            return new Faker().Random.Int(1, 20);
        }

        /// <summary>
        /// Generates an invalid SaleItem quantity (zero or negative).
        /// </summary>
        /// <returns>An invalid quantity.</returns>
        public static int GenerateInvalidQuantity()
        {
            return new Faker().Random.Int(-10, 0);
        }

        /// <summary>
        /// Generates a valid unit price.
        /// </summary>
        /// <returns>A positive unit price.</returns>
        public static decimal GenerateValidUnitPrice()
        {
            return decimal.Parse(new Faker().Commerce.Price(1, 1000));
        }

        /// <summary>
        /// Generates an invalid unit price (negative).
        /// </summary>
        /// <returns>A negative unit price.</returns>
        public static decimal GenerateInvalidUnitPrice()
        {
            return -decimal.Parse(new Faker().Commerce.Price(1, 1000));
        }

        /// <summary>
        /// Generates a valid discount percentage based on quantity rules.
        /// </summary>
        /// <param name="quantity">Quantity of items.</param>
        /// <returns>Appropriate discount percentage.</returns>
        public static decimal GenerateDiscountForQuantity(int quantity)
        {
            if (quantity >= 10) return 20m;
            if (quantity >= 4) return 10m;
            return 0m;
        }
    }
}
