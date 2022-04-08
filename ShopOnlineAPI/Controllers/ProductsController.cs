using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using ShopOnlineAPI.Extensions;
using ShopOnlineAPI.Repositories.Contracts;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                IEnumerable<Entities.Product> products = await _productRepository.GetItems();
                IEnumerable<Entities.ProductCategory> productCategories = await _productRepository.GetCategories();

                if (products is null || productCategories is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(products.ConvertToDto(productCategories));
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data form database.");
            }


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                Entities.Product product = await _productRepository.GetItem(id);
                if (product is null)
                {
                    return BadRequest();
                }
                else
                {
                    Entities.ProductCategory productCategory = await _productRepository.GetCategory(product.CategoryId);

                    ProductDto productDto = product.ConvertToDto(productCategory);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data form the database.");
            }
        }
    }
}
