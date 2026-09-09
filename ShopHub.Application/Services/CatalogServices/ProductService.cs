using AutoMapper;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.ICatalog;
using ShopHub.Application.Specifications;
using ShopHub.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Services.CatalogServices
{
    public class ProductService : IProductService
    {
        private readonly IGenricRepo<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IGenricRepo<Product> productRepo , IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        // -- Method to get all products asynchronously and map them to ProductDto
        public async Task<Pagination<ProductDto>> GetAllProductsAsync(ProductSpecPrams specParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(specParams);

            var count = await _productRepo.CountAsync(spec);

            var products = await _productRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return  new Pagination<ProductDto>(
                specParams.PageIndex,
                specParams.PageSize,
                count,
                data);
        }

        // -- Method to get a product by its ID asynchronously and map it to ProductDto 
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            
            var product = await _productRepo.GetByIdWithSpecAsync(spec);

            if(product is null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }
        
    }
}
