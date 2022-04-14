using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetAllItemsByUserId(HardCode.UserId);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);

            RemoveCartItem(id);
        }

        private CartItemDto GetCartItemItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(s => s.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItemItem(id);

            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}
