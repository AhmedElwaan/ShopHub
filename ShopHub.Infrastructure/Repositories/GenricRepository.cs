using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Specifications;
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

        // ---- Basic CRUD (no spec) ----
        public void Add(T entity) 
            => _dbcontext.Set<T>().Add(entity);

        public void Update(T entity) 
            => _dbcontext.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity) 
            => _dbcontext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync() 
            => await _dbcontext.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) 
            => await _dbcontext.Set<T>().FindAsync(id);

        // ---- Specification-based methods ----
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).ToListAsync();

        public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = _dbcontext.Set<T>().AsQueryable();

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            return await query.CountAsync();
        }

        // ---- Private Helper Method ----
        private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
            => SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>().AsQueryable(), spec);


    }
}
