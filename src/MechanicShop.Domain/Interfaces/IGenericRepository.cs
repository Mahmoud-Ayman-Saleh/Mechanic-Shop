using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Common;

namespace MechanicShop.Domain.Interfaces
{
    public interface IGenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<T?> GetByIdAsync(int id);
            Task<IEnumerable<T>> GetAllAsync();
            Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize);
            Task<T> AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(int id);
            Task<bool> ExistsAsync(int id);
        }
    }
}