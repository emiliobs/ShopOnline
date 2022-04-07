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
            this._context = context;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories() => await _context.ProductCategories.ToListAsync();


        public async Task<ProductCategory> GetCategory(int id) => await _context.ProductCategories.SingleOrDefaultAsync(pc => pc.Id == id);  

        public async Task<Product> GetItem(int id) => await _context.products.FindAsync(id);

        public async Task<IEnumerable<Product>> GetItems() =>await _context.products.ToListAsync();
       
    }
}
