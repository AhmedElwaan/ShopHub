using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.DTOs.CatalogDTOs
{
    public class ProductSpecPrams
    {
        public string? Search { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
