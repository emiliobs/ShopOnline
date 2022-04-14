using Microsoft.EntityFrameworkCore;
using ShopOnline.Models.Dtos;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Entities;
using ShopOnlineAPI.Repositories.Contracts;

namespace ShopOnlineAPI.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ShoppingCartRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddItemAsync(CartItemToAddDto cartItemToAddDto)
        {
            if (await CardItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                CartItem item = await (from product in _context.products
                                       where product.Id == cartItemToAddDto.ProductId
                                       select new CartItem
                                       {
                                           ProductId = product.Id,
                                           CartId = cartItemToAddDto.CartId,
                                           Qty = cartItemToAddDto.Qty,
                                       }).SingleOrDefaultAsync();

                if (item is not null)
                {
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<CartItem> result = await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;

        }

        public async Task<CartItem> DeleteItemByIdAsync(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<IEnumerable<CartItem>> GetAllItemsByUserIdAsync(int userId)
        {
            return await (from cart in this._context.Carts
                          join cartItem in _context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> GetItemByIdAsync(int id)
        {
            return await(from cart in _context.Carts
                         join cartItem in _context.CartItems
                         on cart.Id equals cartItem.CartId
                         where cartItem.Id == id
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             CartId = cartItem.CartId,
                             Qty = cartItem.Qty,
                         }).SingleOrDefaultAsync();
        }

        public Task<CartItem> UpdateQtyAsync(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            throw new NotImplementedException();
        }


        private async Task<bool> CardItemExists(int cartId, int productId)
        {
            return await _context.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }
    }
}
