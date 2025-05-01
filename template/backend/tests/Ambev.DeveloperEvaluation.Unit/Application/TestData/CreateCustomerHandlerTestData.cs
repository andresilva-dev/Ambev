using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using Bogus.Extensions.Brazil;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for CreateCustomerHandler unit tests.
    /// </summary>
    public static class CreateCustomerHandlerTestData
    {
        /// <summary>
        /// Generates a valid CreateCustomerCommand for testing.
        /// </summary>
        public static CreateCustomerCommand GenerateValidCommand()
        {
            var faker = new Faker("pt_BR");

            return new CreateCustomerCommand
            {
                Name = faker.Person.FullName,
                Cpf = faker.Person.Cpf(),
                Email = faker.Internet.Email(),
                Status = CustomerStatus.Active
            };
        }

        /// <summary>
        /// Generates an invalid CreateCustomerCommand with missing and incorrect values.
        /// </summary>
        public static CreateCustomerCommand GenerateInvalidCommand()
        {
            return new CreateCustomerCommand
            {
                Name = "", 
                Cpf = "123456789", 
                Email = "invalid-email", 
                Status = CustomerStatus.Unknown
            };
        }
    }
}
