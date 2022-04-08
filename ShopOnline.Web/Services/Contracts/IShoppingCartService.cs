using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetAllItemsByUserId(int userId);

        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
    }
}
