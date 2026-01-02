using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Application.DTO.Part;
using MechanicShop.Application.DTO.RepairTask;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Entities;

namespace MechanicShop.Application.Interfaces
{
    public interface IRepairTaskService
    {
        Task<PagedResult<RepairTaskDto>> GetAllTasks(int pageNumber, int pageSize);
        Task<IEnumerable<RepairTask>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<PartDto>> GetAssociatedPartsAsync(int taskId);
        Task LinkPartsAsync(int taskId, List<int> partIds);
    }
}