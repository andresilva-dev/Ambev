using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Sale entity class.
    /// Tests cover validation scenarios.
    /// </summary>
    public class SaleTests
    {
        /// <summary>
        /// Tests that validation passes when all sale properties and items are valid.
        /// </summary>
        [Fact(DisplayName = "Validation should pass for valid sale data")]
        public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
        {
            // Arrange
            var sale = new Sale
            {
                CustomerId = Guid.NewGuid(),
                Branch = "Test Branch"
            };

            var product = ProductTestData.GenerateValidProduct();
            sale.AddItem(product.Id, product.Name, 5, product.UnitPrice); 

            // Act
            var result = sale.Validate();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        /// <summary>
        /// Tests that validation fails when sale has no items.
        /// </summary>
        [Fact(DisplayName = "Validation should fail for sale with no items")]
        public void Given_SaleWithoutItems_When_Validated_Then_ShouldReturnInvalid()
        {
            // Arrange
            var sale = new Sale
            {
                CustomerId = Guid.NewGuid(),
                Branch = "Test Branch"
            };

            // Act
            var result = sale.Validate();

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        /// <summary>
        /// Tests that adding an item with invalid quantity throws an exception.
        /// </summary>
        [Theory(DisplayName = "AddItem should throw exception for invalid quantities")]
        [InlineData(0, "Quantity must be greater than zero.")]
        [InlineData(-5, "Quantity must be greater than zero.")]
        [InlineData(21, "Cannot sell more than 20 items of the same product.")]
        [InlineData(100, "Cannot sell more than 20 items of the same product.")]
        public void Given_InvalidItemQuantity_When_AddItem_Then_ShouldThrowArgumentException(int invalidQuantity, string expectedMessage)
        {
            // Arrange
            var sale = new Sale
            {
                CustomerId = Guid.NewGuid(),
                Branch = "Test Branch"
            };

            var product = ProductTestData.GenerateValidProduct();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                sale.AddItem(product.Id, product.Name, invalidQuantity, product.UnitPrice);
            });

            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
