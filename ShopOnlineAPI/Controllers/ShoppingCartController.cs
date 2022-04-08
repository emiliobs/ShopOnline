using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using ShopOnlineAPI.Entities;
using ShopOnlineAPI.Extensions;
using ShopOnlineAPI.Repositories.Contracts;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("{UserId}/GetItemsByUserIdAsync")]
        public async Task<ActionResult<ICollection<CartItemDto>>> GetItemsByUserIdAsync(int userId)
        {
            try
            {
                IEnumerable<Entities.CartItem> cartItems = await _shoppingCartRepository.GetAllItemsByUserIdAsync(userId);
                if (cartItems is null)
                {
                    return NoContent();
                }

                IEnumerable<Entities.Product> products = await _productRepository.GetItems();

                if (products is null)
                {
                    throw new Exception("No products exist in the System.");
                }

                return Ok(cartItems.ConvertToDto(products));

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ICollection<CartItemDto>>> GetItemByIdAsync(int Id)
        {
            try
            {

                Entities.CartItem cartItem = await _shoppingCartRepository.GetItemByIdAsync(Id);
                if (cartItem == null)
                {
                    return NotFound();
                }

                Entities.Product products = await _productRepository.GetItem(cartItem.ProductId);
                if (products == null)
                {
                    return NotFound();
                }

                return Ok(cartItem.ConvertToDto(products));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PoatItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                Entities.CartItem newCartItem = await _shoppingCartRepository.AddItemAsync(cartItemToAddDto);
                if (newCartItem == null)
                {
                    return NoContent();
                }

                Product product = await _productRepository.GetItem(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId}");
                }

                CartItemDto newCarItemDto = newCartItem.ConvertToDto(product);

                return CreatedAtAction(nameof(GetItemByIdAsync), new { id = newCarItemDto.Id }, newCarItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
