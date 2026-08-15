using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
                return await _dbcontext.Set<T>().Include("ProductType").Include("ProductBrand").ToListAsync();
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
