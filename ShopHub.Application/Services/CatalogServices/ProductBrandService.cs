using AutoMapper;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.ICatalog;
using ShopHub.Domain.Entities.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Services.CatalogServices
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IGenricRepo<ProductBrand> _productbrandrepo;
        private readonly IMapper _mapper;

        public ProductBrandService(IGenricRepo<ProductBrand> productbrandrepo, IMapper mapper)
        {
            _productbrandrepo = productbrandrepo;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<ProductBrandDto>> GetAllProductBrandsAsync()
        {
            var productBrands = await _productbrandrepo.GetAllAsync();

            return _mapper.Map<IReadOnlyList<ProductBrandDto>>(productBrands);
        }

        public async Task<ProductBrandDto?> GetProductBrandByIdAsync(int id)
        {
            var productBrand = await _productbrandrepo.GetByIdAsync(id);

            if(productBrand is null)
                return null;

            return _mapper.Map<ProductBrandDto>(productBrand);
        }
    }
}
