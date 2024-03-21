using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
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
    }
}
