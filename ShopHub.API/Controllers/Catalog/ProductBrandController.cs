using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces.ICatalog;

namespace ShopHub.API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandController : ControllerBase
    {
        private readonly IProductBrandService _productBrandService;

        public ProductBrandController(IProductBrandService productBrandService)
        {
            _productBrandService = productBrandService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> GetAllProductBrands()
        {
            var productBrands = await _productBrandService.GetAllProductBrandsAsync();

            return Ok(productBrands);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> GetAllProductBrandById(int id)
        {
            var productBrand = await _productBrandService.GetProductBrandByIdAsync(id);

            if (productBrand is null)
                return NotFound();

            return Ok(productBrand);
        }

    }
}
