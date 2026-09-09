using ShopHub.Application.DTOs.CatalogDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.ICatalog
{
    public interface IProductBrandService
    {
        Task<IReadOnlyList<ProductBrandDto>> GetAllProductBrandsAsync();
        Task<ProductBrandDto?> GetProductBrandByIdAsync(int id);
    }
}
