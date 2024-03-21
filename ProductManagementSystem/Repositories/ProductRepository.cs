using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) {
            _context = context;
        
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;

        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return product;
        }

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products
                                 .Where(p => p.Name.Contains(name))
                                 .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync() 
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products
                                 .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                                 .ToListAsync();
        }


        public async Task<List<Product>> GetSortedProductsAsync(string sortBy, string sortOrder)
        {
            IQueryable<Product> query = _context.Products;

            switch (sortBy.ToLower())
            {
                case "name":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                    break;
                case "category":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(p => p.Category) : query.OrderByDescending(p => p.Category);
                    break;
                case "price":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                    break;
                default:
                    throw new ArgumentException("Invalid sort by value");
            }

            return await query.ToListAsync();
        }

    }
}
