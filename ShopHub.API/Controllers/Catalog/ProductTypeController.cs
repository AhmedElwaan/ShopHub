using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces.ICatalog;

namespace ShopHub.API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {

        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService )
        {
            _productTypeService = productTypeService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductTypeDto>>> GetAllProductTypes(ProductSpecPrams specPrams)
        {
            var productTypes = await _productTypeService.GetAllProductTypesAsync(specPrams);

            return Ok(productTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IReadOnlyList<ProductTypeDto>>> GetAllProductTtpeById(int id)
        {
            var productType = await _productTypeService.GetProductTypeByIdAsync(id);

            if (productType is null)
                return NotFound();

            return Ok(productType);
        }
    }
}
