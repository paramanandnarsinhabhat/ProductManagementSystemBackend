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


        [Fact]
        public async Task GetProduct_ReturnsNotFound_ForInvalidProductId()
        {
            // Arrange
            _mockService.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(0); // Assuming 0 is an invalid ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnsProductDetails_ForValidProductId()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var validProductId = 1;
            var expectedProduct = new Product
            {
                Id = validProductId,
                Name = "Test Product",
                Description = "Test Description",
                Category = "Test Category",
                Price = 100M
            };

            mockService.Setup(s => s.GetProductByIdAsync(validProductId)).ReturnsAsync(expectedProduct);
            var controller = new ProductsController(mockService.Object);

            // Act
            var result = await controller.GetProduct(validProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnedProduct.Id);
            Assert.Equal(expectedProduct.Name, returnedProduct.Name);
            Assert.Equal(expectedProduct.Description, returnedProduct.Description);
            Assert.Equal(expectedProduct.Category, returnedProduct.Category);
            Assert.Equal(expectedProduct.Price, returnedProduct.Price);

            // Optionally verify that the service method was called
            mockService.Verify(s => s.GetProductByIdAsync(validProductId), Times.Once);
        }



    }




}