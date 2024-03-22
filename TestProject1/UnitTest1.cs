namespace TestProject1
{
    using Moq;
    using Xunit;
    using ProductManagementSystem.Controllers;
    using ProductManagementSystem.Models;
    using ProductManagementSystem.Interfaces; // Adjust namespaces as needed
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedAtAction_ForValidProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product" };
            _mockService.Setup(s => s.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _controller.PostProduct(product);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(product.Id, returnValue.Id);
        }
    }



}