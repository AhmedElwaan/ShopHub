using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Domain.Entities.Catalog.Domain
{
    public class ProductBrand : BaseEntities
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
