using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class UpdateSaleHandlerTestData
    {
        public static UpdateSaleCommand GenerateValidCommand(int quantityItems)
        {
            var itemFaker = new Faker<UpdateSaleItemCommand>()
                 .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                 .RuleFor(i => i.Quantity, f => quantityItems);

            return new Faker<UpdateSaleCommand>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => itemFaker.Generate(1))
                .Generate();
        }

        /// <summary>
        /// Generates a valid UpdateSaleItemCommand with specific quantity if provided.
        /// </summary>
        public static UpdateSaleItemCommand GenerateValidItemCommand(int? quantity = null)
        {
            return new Faker<UpdateSaleItemCommand>()
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => quantity ?? f.Random.Int(1, 3))
                .Generate();
        }

        /// <summary>
        /// Generates an invalid UpdateSaleCommand with empty fields.
        /// </summary>
        public static UpdateSaleCommand GenerateInvalidCommand()
        {
            return new UpdateSaleCommand
            {
                CustomerId = Guid.Empty,
                Branch = "",
                Items = new List<UpdateSaleItemCommand>()
            };
        }

        public static UpdateSaleCommand GenerateUpdateCommandCancelledSale(int quantityItems)
        {
            var itemFaker = new Faker<UpdateSaleItemCommand>()
                 .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                 .RuleFor(i => i.Quantity, f => quantityItems);

            return new Faker<UpdateSaleCommand>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => itemFaker.Generate(1))
                .RuleFor(s => s.Cancelled, true)
                .Generate();
        }
    }
}
