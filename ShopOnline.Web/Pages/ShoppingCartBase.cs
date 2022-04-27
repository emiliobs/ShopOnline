using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string TotalPrice { get; set; }

        public int TotalQuantity { get; set; }

        public string ErrorMessage { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetAllItemsByUserId(HardCode.UserId);
                CartChanged();
                //CalculateCartSummaryTotal();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    CartItemQtyUpdateDto updateItemDto = new()
                    {
                        CartItemId = id,
                        Qty = qty,
                    };

                    CartItemDto returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    //CalculateCartSummaryTotal();

                    CartChanged();

                    await MakeUpdateQtyButtonVisible(id, false);
                }
                else
                {
                    CartItemDto item = ShoppingCartItems.FirstOrDefault(sci => sci.Id == id);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            _ = await ShoppingCartService.DeleteItem(id);

            RemoveCartItem(id);
            CartChanged();
            //CalculateCartSummaryTotal();
        }

        private CartItemDto GetCartItemItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(s => s.Id == id);
        }

        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }

        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }

        private void RemoveCartItem(int id)
        {
            CartItemDto cartItemDto = GetCartItemItem(id);

            _ = ShoppingCartItems.Remove(cartItemDto);
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            CartItemDto item = GetCartItemItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

        private void CalculateCartSummaryTotal()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }


        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(s => s.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(s => s.Qty);
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotal();
            ShoppingCartService.RaiseEventOnShoppingCartChange(TotalQuantity);
        }
    }
}
