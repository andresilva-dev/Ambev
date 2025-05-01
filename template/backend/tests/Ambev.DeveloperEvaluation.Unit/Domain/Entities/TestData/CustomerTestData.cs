using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data for the Customer entity using the Bogus library.
    /// This class centralizes test data generation for consistency across test cases.
    /// </summary>
    public static class CustomerTestData
    {
        /// <summary>
        /// Faker configuration for generating valid Customer entities.
        /// </summary>
        private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
            .RuleFor(c => c.Cpf, f => GenerateValidCpf())
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Status, f => f.PickRandom(Enum.GetValues<CustomerStatus>().Where(s => s != CustomerStatus.Unknown)))
            .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(c => c.UpdatedAt, f => null);

        /// <summary>
        /// Generates a valid Customer entity with randomized data.
        /// </summary>
        /// <returns>A valid Customer entity.</returns>
        public static Customer GenerateValidCustomer()
        {
            return CustomerFaker.Generate();
        }

        /// <summary>
        /// Generates a valid CPF in a basic format (not validated against official algorithm).
        /// </summary>
        /// <returns>A string representing a valid-looking CPF.</returns>
        public static string GenerateValidCpf()
        {
            var faker = new Faker();
            var random = faker.Random;
            var baseCpf = new int[9];

            for (int i = 0; i < 9; i++)
                baseCpf[i] = random.Int(0, 9);

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += baseCpf[i] * (10 - i);

            int firstDigit = sum % 11;
            firstDigit = firstDigit < 2 ? 0 : 11 - firstDigit;

            sum = 0;
            for (int i = 0; i < 9; i++)
                sum += baseCpf[i] * (11 - i);
            sum += firstDigit * 2;

            int secondDigit = sum % 11;
            secondDigit = secondDigit < 2 ? 0 : 11 - secondDigit;

            var cpf = string.Concat(baseCpf) + firstDigit + secondDigit;

            return cpf;
        }

        /// <summary>
        /// Generates an invalid CPF (e.g., incorrect length).
        /// </summary>
        /// <returns>An invalid CPF string.</returns>
        public static string GenerateInvalidCpf()
        {
            return new Faker().Random.ReplaceNumbers("#####");
        }

        /// <summary>
        /// Generates an invalid email for testing purposes.
        /// </summary>
        public static string GenerateInvalidEmail()
        {
            return new Faker().Lorem.Word();
        }

        /// <summary>
        /// Generates a name that exceeds expected limits.
        /// </summary>
        public static string GenerateLongName()
        {
            return new Faker().Random.String2(101);
        }
    }
}
