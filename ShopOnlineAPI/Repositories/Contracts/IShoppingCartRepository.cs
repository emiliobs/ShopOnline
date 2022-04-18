using ShopOnline.Models.Dtos;
using ShopOnlineAPI.Entities;

namespace ShopOnlineAPI.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItemAsync(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateQtyAsync(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<CartItem> DeleteItemByIdAsync(int id);
        Task<CartItem> GetItemByIdAsync(int id);
        Task<IEnumerable<CartItem>> GetAllItemsByUserIdAsync(int userId);
        Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
    }
}
