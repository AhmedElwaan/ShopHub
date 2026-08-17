using ShopHub.Application.DTOs.CatalogDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.ICatalog
{
    public interface IProductTypeService
    {
        Task<IReadOnlyList<ProductTypeDto>> GetAllProductTypesAsync();
        Task<ProductTypeDto?> GetProductTypeByIdAsync(int id);
    }
}
