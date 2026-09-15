using ProductInventory.Models;

namespace ProductInventory.Tests.Models
{
    public class ProductViewModelTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void IsInStock_VariousQuantities_ReturnsExpectedResult(int quantity, bool expected)
        {
            // Arrange
            var product = new ProductViewModel { Quantity = quantity };

            // Act
            var result = product.IsInStock;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Bad()
        {
            Assert.Fail("This test is intentionally failing to demonstrate a failing test case.");
        }
    }
}
