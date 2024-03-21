using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;

namespace ProductManagementSystem.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            return await _productRepository.AddProductAsync(product);
        }

        public async Task<List<Product>> GetAllProductsAsync() 
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id) 
        {
            return await _productRepository.GetProductByIdAsync(id);
        }
    }
}
