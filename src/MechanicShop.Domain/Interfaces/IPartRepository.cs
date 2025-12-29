using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Entities;

namespace MechanicShop.Domain.Interfaces
{
    public interface IPartRepository : IGenericRepository
    {
        public Task<(IEnumerable<Part> parts, int totalCount)> GetAllPartsAsync(string? category = null, string? supplier = null, string? search = null);
        public Task AddAsync(Part part);
        public Task<Part?> GetByIdAsync(int id);
        public Task UpdateAsync(Part part);
        public Task DeleteAsync(int id);

    }
}
