using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library for product updates.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class UpdateProductHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid UpdateProductCommand instances.
        /// The generated products will have valid:
        /// - Id (non-empty GUID)
        /// - Name (between 3 and 100 characters)
        /// - UnitPrice (greater than zero)
        /// </summary>
        private static readonly Faker<UpdateProductCommand> updateProductHandlerFaker = new Faker<UpdateProductCommand>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => Math.Round(f.Random.Decimal(1, 500), 2))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        /// <summary>
        /// Configures the Faker to generate valid UpdateProductCommand instances.
        /// The generated products will have valid:
        /// - Id (non-empty GUID)
        /// - Name (between 3 and 100 characters)
        /// - UnitPrice (greater than zero)
        /// </summary>
        private static readonly Faker<Product> updateProductFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => Math.Round(f.Random.Decimal(1, 500), 2))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        /// <summary>
        /// Generates a valid UpdateProductCommand with randomized data.
        /// The generated command will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid UpdateProductCommand with randomly generated data.</returns>
        public static UpdateProductCommand GenerateValidCommand()
        {
            return updateProductHandlerFaker.Generate();
        }

        /// <summary>
        /// Generates a valid Product.
        /// The generated command will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid Product with randomly generated data.</returns>
        public static Product GenerateValidProduct()
        {
            return updateProductFaker.Generate();
        }

    }
}
