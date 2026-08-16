using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces.ICatalog;

namespace ShopHub.API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts([FromQuery] ProductSpecPrams specPrams)
        {
            var products = await _productService.GetAllProductsAsync(specPrams);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProductsById(int id)
        {
            var products = await _productService.GetProductByIdAsync(id);

            if(products is null)
                return NotFound();

            return Ok(products);
        }
    }
}
