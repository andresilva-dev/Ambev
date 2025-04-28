using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library for product creation.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class CreateProductHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid CreateProductCommand instances.
        /// The generated products will have valid:
        /// - Name (between 3 and 100 characters)
        /// - UnitPrice (greater than zero)
        /// </summary>
        private static readonly Faker<CreateProductCommand> createProductHandlerFaker = new Faker<CreateProductCommand>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => Math.Round(f.Random.Decimal(1, 500), 2))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        /// <summary>
        /// Generates a valid CreateProductCommand with randomized data.
        /// The generated command will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
        public static CreateProductCommand GenerateValidCommand()
        {
            return createProductHandlerFaker.Generate();
        }
    }
}
