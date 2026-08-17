using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Application.Specifications;
using ShopHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces
{
    public interface IGenricRepo<T> where T : BaseEntities
    {

        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        
        
    }
}
