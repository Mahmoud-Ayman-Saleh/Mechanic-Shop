using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Entities;

namespace MechanicShop.Application.Interfaces
{
    public interface IRepairTaskService
    {
        Task<IEnumerable<RepairTask>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<Part>> GetAssociatedPartsAsync(int taskId);
        Task LinkPartsAsync(int taskId, List<int> partIds);
    }
}