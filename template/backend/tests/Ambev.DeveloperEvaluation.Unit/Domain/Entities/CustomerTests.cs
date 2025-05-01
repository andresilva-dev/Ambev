using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Customer entity class.
    /// Tests cover status transitions and validation scenarios.
    /// </summary>
    public class CustomerTests
    {
        /// <summary>
        /// Tests that a suspended customer becomes active when activated.
        /// </summary>
        [Fact(DisplayName = "Customer status should change to Active when activated")]
        public void Given_SuspendedCustomer_When_Activated_Then_StatusShouldBeActive()
        {
            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Status = CustomerStatus.Suspended;

            customer.Activate();

            Assert.Equal(CustomerStatus.Active, customer.Status);
        }

        /// <summary>
        /// Tests that an active customer becomes suspended when suspended.
        /// </summary>
        [Fact(DisplayName = "Customer status should change to Suspended when suspended")]
        public void Given_ActiveCustomer_When_Suspended_Then_StatusShouldBeSuspended()
        {
            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Status = CustomerStatus.Active;

            customer.Suspend();

            Assert.Equal(CustomerStatus.Suspended, customer.Status);
        }

        /// <summary>
        /// Tests that an inactive customer can be activated again.
        /// </summary>
        [Fact(DisplayName = "Customer status should change to Active from Inactive")]
        public void Given_InactiveCustomer_When_Activated_Then_StatusShouldBeActive()
        {
            var customer = CustomerTestData.GenerateValidCustomer();
            customer.Status = CustomerStatus.Inactive;

            customer.Activate();

            Assert.Equal(CustomerStatus.Active, customer.Status);
        }

        /// <summary>
        /// Tests that validation succeeds for valid customer data.
        /// </summary>
        [Fact(DisplayName = "Validation should pass for valid customer data")]
        public void Given_ValidCustomerData_When_Validated_Then_ShouldReturnValid()
        {
            var customer = CustomerTestData.GenerateValidCustomer();

            var result = customer.Validate();

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        /// <summary>
        /// Tests that validation fails for invalid customer data.
        /// </summary>
        [Fact(DisplayName = "Validation should fail for invalid customer data")]
        public void Given_InvalidCustomerData_When_Validated_Then_ShouldReturnInvalid()
        {
            var customer = new Customer
            {
                Cpf = CustomerTestData.GenerateInvalidCpf(),
                Name = "",
                Email = CustomerTestData.GenerateInvalidEmail(),
                Status = CustomerStatus.Unknown
            };

            var result = customer.Validate();

            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }
    }
}
