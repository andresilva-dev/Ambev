using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class UpdateUserHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid UpdateUserCommand instances.
        /// The generated commands will have valid:
        /// - Id (GUID)
        /// - Username (internet-style username)
        /// - Password (complex format)
        /// - Email (valid email format)
        /// - Phone (Brazilian format)
        /// - Status (Active or Suspended)
        /// - Role (Customer or Admin)
        /// </summary>
        private static readonly Faker<UpdateUserCommand> updateUserHandlerFaker = new Faker<UpdateUserCommand>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin));

        /// <summary>
        /// Generates a valid UpdateUserCommand with randomized data.
        /// All fields are populated with values that meet validation rules.
        /// </summary>
        /// <returns>A valid UpdateUserCommand instance.</returns>
        public static UpdateUserCommand GenerateValidCommand()
        {
            return updateUserHandlerFaker.Generate();
        }
    }
}
