using Microsoft.EntityFrameworkCore;
using ShopHub.Domain.Entities.Catalog;
using ShopHub.Domain.Entities.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.Data.SeedData
{
    public static class DataSeeder
    {
        public static async Task Seeder(AppDbContext dbcontext)
        {
            // -- Seed Product Brands 
            if (!await dbcontext.ProductBrands.AnyAsync()) 
            {
                var brandjson = await File.ReadAllTextAsync(
                    "E:\\Elwan\\.NET\\ShopHub\\ShopHub.Infrastructure\\Data\\SeedData\\ProductBrand.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandjson);

                if(brands is not null && brands.Count > 0)
                {
                    await dbcontext.ProductBrands.AddRangeAsync(brands);
                }
            }

            // -- Seed Product Types
            if (!await dbcontext.ProductTypes.AnyAsync())
            {
                var typejson = await File.ReadAllTextAsync(
                    "E:\\Elwan\\.NET\\ShopHub\\ShopHub.Infrastructure\\Data\\SeedData\\ProductType.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typejson);

                if (types is not null && types.Count > 0)
                {
                    await dbcontext.ProductTypes.AddRangeAsync(types);
                }
            }

            // -- Seed Product 
            if (!await dbcontext.Products.AnyAsync())
            {
                var productjson = await File.ReadAllTextAsync(
                    "E:\\Elwan\\.NET\\ShopHub\\ShopHub.Infrastructure\\Data\\SeedData\\Product.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productjson);

                if (products is not null && products.Count > 0)
                {
                    await dbcontext.Products.AddRangeAsync(products);
                }
            }
            await dbcontext.SaveChangesAsync();
        }
    }
}
