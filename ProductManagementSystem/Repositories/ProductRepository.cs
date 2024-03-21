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

        public Task<Product> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
