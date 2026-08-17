using AutoMapper;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.ICatalog;
using ShopHub.Application.Specifications;
using ShopHub.Domain.Entities.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Services.CatalogServices
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IGenricRepo<ProductType> _producyTypeRepo;
        private readonly IMapper _mapper;

        public ProductTypeService(IGenricRepo<ProductType> producyTypeRepo , IMapper mapper )
        {
            _producyTypeRepo = producyTypeRepo;
            _mapper = mapper;
        }

        // -- Method to get all product types asynchronously and map them to ProductTypeDto
        public async Task<IReadOnlyList<ProductTypeDto>> GetAllProductTypesAsync()
        {

           var productTypes = await _producyTypeRepo.GetAllAsync();

            return _mapper.Map<IReadOnlyList<ProductTypeDto>>(productTypes);
        }

        // -- Method to get a product type by its ID asynchronously and map it to ProductTypeDto
        public async Task<ProductTypeDto?> GetProductTypeByIdAsync(int id)
        {
            var productType =await _producyTypeRepo.GetByIdAsync(id);

            if(productType is null)
                return null;

            return _mapper.Map<ProductTypeDto>(productType);
        }
    }
}
