using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Interfaces;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
       private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        // POST api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            try
            {
                var createdProduct = await _productService.AddProductAsync(product);

                // Return an HTTP 201 Created response with the created product
                return CreatedAtAction(nameof(PostProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
             
                // Return a generic HTTP 500 Internal Server Error response
                // if something goes wrong beyond the client's control
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {

                // Return a generic error response such as 500 Internal Server Error
                // It's often best practice not to expose detailed error information in production environments
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(); 
                }
                return Ok(product); 
            }
            catch (Exception ex)
            {
                // Log the exception details here using your preferred logging approach
                // It's often best practice not to expose detailed exception information in production environments
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the product");
            }
        }



        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter is required");
            }

            try
            {
                var products = await _productService.GetProductsByNameAsync(name);
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found with the name '{name}'");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception here using your preferred logging mechanism.

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("total-count")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            try
            {
                var totalCount = await _productService.GetTotalCountAsync();
                return Ok(totalCount); // Returns HTTP 200 OK with the total count
            }
            catch (Exception ex)
            {
                // Log the exception here using your preferred logging mechanism
               return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string category)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(category);
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found in the category '{category}'.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception here using your preferred logging mechanism

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("sort")]
        public async Task<ActionResult<List<Product>>> SortProducts([FromQuery] string sortBy = "name", [FromQuery] string sortOrder = "asc")
        {
            try
            {
                var sortedProducts = await _productService.GetSortedProductsAsync(sortBy, sortOrder);
                return Ok(sortedProducts);
            }
            catch (ArgumentException ex)
            {
                // If an invalid "sortBy" argument is passed
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response


                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }

            try
            {
                var updateSuccess = await _productService.UpdateProductAsync(product);
                if (!updateSuccess)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                // Log the exception details here using your preferred logging approach.
                 // Return a generic error response such as 500 Internal Server Error.
                // best practice not to expose detailed error information in production environments.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleteSuccess = await _productService.DeleteProductAsync(id);
                if (!deleteSuccess)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                // Log the exception here using your preferred logging mechanism
                
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllProducts()
        {
            try
            {
                await _productService.DeleteAllProductsAsync();
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                // Log the exception here using your preferred logging mechanism
                // Example: _logger.LogError(ex, "An error occurred while deleting all products.");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }





    }
}
