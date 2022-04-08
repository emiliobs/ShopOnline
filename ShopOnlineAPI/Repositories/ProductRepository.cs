using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Entities;
using ShopOnlineAPI.Repositories.Contracts;

namespace ShopOnlineAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ProductRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            return await _context.ProductCategories.SingleOrDefaultAsync(pc => pc.Id == id);
        }

        public async Task<Product> GetItem(int id)
        {
            return await _context.products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await _context.products.ToListAsync();
        }
    }
}
