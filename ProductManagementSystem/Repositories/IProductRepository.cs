﻿using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);

        Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductsByNameAsync(string name);

        Task<int> GetTotalCountAsync();

        Task<List<Product>> GetProductsByCategoryAsync(string category);
    }
}
