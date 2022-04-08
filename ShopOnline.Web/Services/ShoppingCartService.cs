using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CartItemDto>> GetAllItemsByUserId(int userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItemsByUserIdAsync");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                }
                else
                {
                    string message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} Message: {message}");

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ShoppingCart", cartItemToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    string message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message  - { message }");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
