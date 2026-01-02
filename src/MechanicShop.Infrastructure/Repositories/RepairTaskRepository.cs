using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Entities;
using MechanicShop.Domain.Interfaces;
using MechanicShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MechanicShop.Infrastructure.Repositories
{
    public class RepairTaskRepository : GenericRepository<RepairTask>, IRepairTaskRepository
    {
        public RepairTaskRepository(MechanicShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Part>> GetAssociatedPartsAsync(int taskId)
        {
            var repairTask = await _context.RepairTasks
                .AsNoTracking()
                .Include(rt => rt.Parts)
                .FirstOrDefaultAsync(rt => rt.Id == taskId);

            if (repairTask == null)
            {
                Console.WriteLine($"\n\n###################\n\nRepairTask with ID {taskId} not found.##################\n\n");
                return new List<Part>();
            }

            return repairTask.Parts ?? new List<Part>();
        }

        public async Task LinkPartsAsync(int taskId, List<int> partIds)
        {
            var repairTask = await _context.RepairTasks
                .Include(rt => rt.Parts)
                .FirstOrDefaultAsync(rt => rt.Id == taskId);

            if (repairTask == null)
                throw new KeyNotFoundException($"RepairTask with ID {taskId} not found.");

            var partsToLink = await _context.Parts
                .Where(p => partIds.Contains(p.Id))
                .ToListAsync();

            if (partsToLink.Count != partIds.Count)
                throw new ArgumentException("One or more part IDs are invalid.");

            // Add only new parts that aren't already linked
            foreach (var part in partsToLink)
            {
                if (!repairTask.Parts.Any(p => p.Id == part.Id))
                {
                    repairTask.Parts.Add(part);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RepairTask>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _dbSet.Where(rt => !rt.IsDeleted).ToListAsync();

            var searchLower = searchTerm.ToLower();
            return await _dbSet
                .Where(rt => !rt.IsDeleted && rt.Name.ToLower().Contains(searchLower))
                .ToListAsync();
        }
    }
}