using ShopHub.Application.DTOs.CatalogDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.ICatalog
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        
    }
}
