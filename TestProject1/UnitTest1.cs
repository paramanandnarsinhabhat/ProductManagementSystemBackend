namespace TestProject1
{
    using Moq;
    using Xunit;
    using ProductManagementSystem.Controllers;
    using ProductManagementSystem.Models;
    using ProductManagementSystem.Interfaces; // Adjust namespaces as needed
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

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
        public async Task PostProduct_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var product = new Product { Name = "Error Product", Description = "Error Description", Category = "Error", Price = 0M };
            _mockService.Setup(s => s.AddProductAsync(It.IsAny<Product>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.PostProduct(product);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Test exception", statusCodeResult.Value);
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
                Price = 10M
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


        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" }
        };
            _mockService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count);
            Assert.Equal(products, returnedProducts);

            _mockService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }


        [Fact]
        public async Task GetProducts_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllProductsAsync()).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while retrieving products", statusCodeResult.Value);

            _mockService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchProducts_ReturnsBadRequest_ForMissingOrEmptyName(string name)
        {
            // Act
            var result = await _controller.SearchProducts(name);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task SearchProducts_ReturnsNotFound_WhenNoProductsFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetProductsByNameAsync(It.IsAny<string>())).ReturnsAsync(new List<Product>());

            // Act
            var result = await _controller.SearchProducts("NonExistingProduct");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }


        [Fact]
        public async Task SearchProducts_ReturnsOk_WithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Name = "Test Product" } };
            _mockService.Setup(s => s.GetProductsByNameAsync("Test")).ReturnsAsync(products);

            // Act
            var result = await _controller.SearchProducts("Test");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnedProducts);
        }

        [Fact]
        public async Task SearchProducts_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockService.Setup(s => s.GetProductsByNameAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.SearchProducts("Error");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetTotalCount_ReturnsOk_WithTotalCount()
        {
            // Arrange
            var totalCount = 5;
            _mockService.Setup(s => s.GetTotalCountAsync()).ReturnsAsync(totalCount);

            // Act
            var result = await _controller.GetTotalCount();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(totalCount, okResult.Value);
        }

        [Fact]
        public async Task GetTotalCount_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockService.Setup(s => s.GetTotalCountAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetTotalCount();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetProductsByCategory_ReturnsOk_WithProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
    {
        new Product { Id = 1, Name = "Product A", Category = "Electronics", Price = 200 },
        new Product { Id = 2, Name = "Product B", Category = "Electronics", Price = 150 }
    };
            _mockService.Setup(service => service.GetProductsByCategoryAsync("Electronics")).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetProductsByCategory("Electronics");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GetProductsByCategory_ReturnsNotFound_WhenNoProductsFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetProductsByCategoryAsync("Nonexistent")).ReturnsAsync(new List<Product>());

            // Act
            var result = await _controller.GetProductsByCategory("Nonexistent");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No products found in the category 'Nonexistent'.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProductsByCategory_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockService.Setup(service => service.GetProductsByCategoryAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetProductsByCategory("ErrorCategory");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }


        [Fact]
        public async Task SortProducts_ReturnsOk_WithSortedProducts()
        {
            // Arrange: Ensure this list matches the expected order in your assertions.
            var mockProducts = new List<Product>
    {
        new Product { Id = 2, Name = "A Product", Category = "Electronics", Price = 150 },
        new Product { Id = 1, Name = "B Product", Category = "Electronics", Price = 200 }
    };
            _mockService.Setup(service => service.GetSortedProductsAsync("name", "asc")).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.SortProducts("name", "asc");

            // Assert: Ensure the assertion matches the setup.
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
            Assert.Equal("A Product", products.First().Name); // This should now pass.
        }



    }




}