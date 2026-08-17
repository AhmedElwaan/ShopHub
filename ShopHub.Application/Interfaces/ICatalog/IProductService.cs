using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Specifications;
using ShopHub.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.ICatalog
{
    public interface IProductService
    {
        Task<Pagination<ProductDto>> GetAllProductsAsync(ProductSpecPrams specParams);
        Task<ProductDto?> GetProductByIdAsync(int id);
        
    }
}
