using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager  NavigationManager { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await ProductService.GetItems();

                var shoppingCartItems = await ShoppingCartService.GetAllItemsByUserId(HardCode.UserId);
                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChange(totalQty);

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }


        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetgroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId
                                        into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupeProductDtos)
        {
            return groupeProductDtos.FirstOrDefault(pg => pg.CategoryId == groupeProductDtos.Key).CategoryName;
        }
    }
}
