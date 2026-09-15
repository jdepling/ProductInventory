using System.ComponentModel.DataAnnotations;
using ProductInventory.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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
        public void Quantity_NegativeQuantity_FailsValidation()
        {
            // Arrange
            var product = new ProductViewModel { Quantity = -1 };

            // Act
            var result = ValidateModel(product);
            var errorMessageForQuantity = result
                .Where(r => r.MemberNames.Contains(nameof(ProductViewModel.Quantity)))
                .Select(r => r.ErrorMessage)
                .FirstOrDefault();

            // Assert
            Assert.Equal("Quantity cannot be negative", errorMessageForQuantity);
        }

        [Fact]
        public void Name_EmptyName_FailsValidation()
        {
            // Arrange
            var product = new ProductViewModel { Name = "" };

            // Act
            var result = ValidateModel(product);
            var errorMessageForName = result
                .Where(r => r.MemberNames.Contains(nameof(ProductViewModel.Name)))
                .Select(r => r.ErrorMessage)
                .FirstOrDefault();

            // Assert
            Assert.Equal("Product name is required", errorMessageForName);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

    }
}

       