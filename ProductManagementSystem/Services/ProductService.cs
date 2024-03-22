using ProductManagementSystem.Interfaces;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;

namespace ProductManagementSystem.Services
{
    public class ProductService : IProductService
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

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return await _productRepository.GetProductsByNameAsync(name);
        }


        public async Task<int> GetTotalCountAsync() 
        {
            return await _productRepository.GetTotalCountAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _productRepository.GetProductsByCategoryAsync(category);
        }

        public async Task<List<Product>> GetSortedProductsAsync(string sortBy, string sortOrder)
        {
            return await _productRepository.GetSortedProductsAsync(sortBy, sortOrder);
        }


        public async Task<bool> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteProductAsync(productId);
        }

        public async Task DeleteAllProductsAsync()
        {
            await _productRepository.DeleteAllProductsAsync();
        }


    }
}
