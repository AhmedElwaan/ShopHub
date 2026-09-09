using ShopHub.Domain.Entities.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Domain.Entities.Catalog
{
    public class Product : BaseEntities
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }
        public int ProductTypeId { get; set; } // Foreign key to ProductType
        public ProductType ProductType { get; set; } // Navigation property to ProductType
        public int ProductBrandId { get; set; } // Foreign key to ProductBrand
        public ProductBrand ProductBrand { get; set; } // Navigation property to ProductBrand
        
    }
}
