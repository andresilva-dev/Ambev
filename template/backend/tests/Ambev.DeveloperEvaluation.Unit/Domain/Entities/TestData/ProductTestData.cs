using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class ProductTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid Product entities.
        /// The generated products will have valid:
        /// - Name (non-empty, realistic product names)
        /// - Description (short realistic description)
        /// - UnitPrice (positive decimal)
        /// - CreatedAt (current UTC time)
        /// </summary>
        private static readonly Faker<Product> ProductFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(1, 1000)))
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.UpdatedAt, f => null);

        /// <summary>
        /// Generates a valid Product entity with randomized data.
        /// The generated product will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid Product entity with randomly generated data.</returns>
        public static Product GenerateValidProduct()
        {
            return ProductFaker.Generate();
        }

        /// <summary>
        /// Generates a valid product name using Faker.
        /// The generated name will:
        /// - Be non-empty
        /// - Resemble a real product name
        /// </summary>
        /// <returns>A valid product name.</returns>
        public static string GenerateValidProductName()
        {
            return new Faker().Commerce.ProductName();
        }

        /// <summary>
        /// Generates a valid product description using Faker.
        /// The generated description will:
        /// - Be non-empty
        /// - Resemble a real product description
        /// </summary>
        /// <returns>A valid product description.</returns>
        public static string GenerateValidProductDescription()
        {
            return new Faker().Commerce.ProductDescription();
        }

        /// <summary>
        /// Generates a valid unit price.
        /// The generated unit price will:
        /// - Be a positive decimal number
        /// - Between 1 and 1000
        /// </summary>
        /// <returns>A valid unit price.</returns>
        public static decimal GenerateValidUnitPrice()
        {
            return decimal.Parse(new Faker().Commerce.Price(1, 1000));
        }

        /// <summary>
        /// Generates an invalid product name for testing negative scenarios.
        /// The generated name will:
        /// - Be an empty string
        /// This is useful for testing name validation error cases.
        /// </summary>
        /// <returns>An invalid product name (empty string).</returns>
        public static string GenerateInvalidProductName()
        {
            return string.Empty;
        }

        /// <summary>
        /// Generates an invalid product description for testing negative scenarios.
        /// The generated description will:
        /// - Be an empty string
        /// This is useful for testing description validation error cases.
        /// </summary>
        /// <returns>An invalid product description (empty string).</returns>
        public static string GenerateInvalidProductDescription()
        {
            return string.Empty;
        }

        /// <summary>
        /// Generates an invalid unit price for testing negative scenarios.
        /// The generated unit price will:
        /// - Be a negative decimal number
        /// This is useful for testing price validation error cases.
        /// </summary>
        /// <returns>An invalid (negative) unit price.</returns>
        public static decimal GenerateInvalidUnitPrice()
        {
            return -new Faker().Random.Decimal(1, 1000);
        }
    }
}
