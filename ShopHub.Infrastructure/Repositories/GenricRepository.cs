using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Domain.Entities;
using ShopHub.Domain.Entities.Catalog;
using ShopHub.Domain.Entities.Catalog.Domain;
using ShopHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.Repositories
{
    public class GenricRepository<T> : IGenricRepo<T> where T : BaseEntities
    {
        private readonly Data.AppDbContext _dbcontext;

        public GenricRepository(Data.AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(ProductSpecPrams specPrams)
        {

            if (typeof(T) == typeof(Product))
            {
                IQueryable<Product> query = _dbcontext.Set<Product>();
                 
                if (specPrams.TypeId.HasValue)
                {
                    query = query.Where(e => e.ProductTypeId == specPrams.TypeId.Value);
                }

                if (specPrams.BrandId.HasValue)
                {
                    query = query.Where(e => e.ProductBrandId == specPrams.BrandId.Value);
                }

                if(!string.IsNullOrEmpty(specPrams.Search))
                {
                    query = query.Where(e => e.Name.Contains(specPrams.Search));
                }

                if(specPrams.Sort == "price")
                {
                    query = query.OrderBy(e => e.Price);
                }
                else if(specPrams.Sort == "priceDesc")
                {
                    query = query.OrderByDescending(e => e.Price);
                }
                else
                {
                    query = query.OrderBy(e => e.Name);
                }
                query = query.Skip((specPrams.PageIndex - 1) * specPrams.PageSize).Take(specPrams.PageSize);

                query = query.Include("ProductType").Include("ProductBrand");

                return await query.ToListAsync() as IReadOnlyList<T>;   


            }
            return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _dbcontext.Set<T>()
                    .Include("ProductType")
                    .Include("ProductBrand")
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            return await _dbcontext.Set<T>().FindAsync(id);
        }
        public void AddAsync(T entity)
        {
            _dbcontext.Set<T>().Add(entity);
        }
        public void DeleteAsync(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
        }
        public void UpdateAsync(T entity)
        {
            _dbcontext.Entry(entity).State = EntityState.Modified;
        }
    }
}
