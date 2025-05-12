using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library for sale creation.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provides both valid and invalid data scenarios.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        /// <summary>
        /// Generates a valid CreateSaleCommand with optional quantity for each item.
        /// If quantity is specified, all items will have that quantity.
        /// </summary>
        /// <param name="quantity">Optional fixed quantity to apply to all items.</param>
        /// <returns>A valid CreateSaleCommand with consistent item quantities.</returns>
        public static CreateSaleCommand GenerateValidCommand(int quantity)
        {
            var itemFaker = new Faker<CreateSaleItemCommand>()
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => quantity);

            return new Faker<CreateSaleCommand>()
                .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => itemFaker.Generate(1))
                .Generate();
        }

        /// <summary>
        /// Generates a valid CreateSaleItemCommand with specific quantity if provided.
        /// </summary>
        public static CreateSaleItemCommand GenerateValidItemCommand(int? quantity = null)
        {
            return new Faker<CreateSaleItemCommand>()
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => quantity ?? f.Random.Int(1, 3))
                .Generate();
        }

        /// <summary>
        /// Generates a CreateSaleCommand with item(s) eligible for 10% discount (4 to 9 units).
        /// </summary>
        public static CreateSaleCommand GenerateCommandWith10PercentDiscount()
        {
            var items = new List<CreateSaleItemCommand>
            {
                GenerateValidItemCommand(quantity: new Random().Next(4, 9))
            };

            return new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                Branch = new Bogus.DataSets.Company().CompanyName(),
                Items = items
            };
        }

        /// <summary>
        /// Generates a CreateSaleCommand with item(s) eligible for 20% discount (10 to 20 units).
        /// </summary>
        public static CreateSaleCommand GenerateCommandWith20PercentDiscount()
        {
            var items = new List<CreateSaleItemCommand>
            {
                GenerateValidItemCommand(quantity: new Random().Next(10, 20))
            };

            return new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                Branch = new Bogus.DataSets.Company().CompanyName(),
                Items = items
            };
        }

        /// <summary>
        /// Generates an invalid CreateSaleCommand where item quantity exceeds the maximum allowed (above 20).
        /// </summary>
        public static CreateSaleCommand GenerateCommandWithQuantityAboveAllowed()
        {
            var items = new List<CreateSaleItemCommand>
            {
                GenerateValidItemCommand(quantity: new Random().Next(21, 30))
            };

            return new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                Branch = new Bogus.DataSets.Company().CompanyName(),
                Items = items
            };
        }

        /// <summary>
        /// Generates an invalid CreateSaleCommand with empty fields.
        /// </summary>
        public static CreateSaleCommand GenerateInvalidCommand()
        {
            return new CreateSaleCommand
            {
                CustomerId = Guid.Empty,
                Branch = "",
                Items = new List<CreateSaleItemCommand>()
            };
        }
    }
}
