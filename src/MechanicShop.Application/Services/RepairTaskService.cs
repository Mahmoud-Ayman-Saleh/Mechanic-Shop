using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Application.DTO.Part;
using MechanicShop.Application.DTO.RepairTask;
using MechanicShop.Application.Interfaces;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Entities;
using MechanicShop.Domain.Interfaces;

namespace MechanicShop.Application.Services
{
    public class RepairTaskService : IRepairTaskService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepairTaskRepository _repairTaskRepository;

        public RepairTaskService(IUnitOfWork unitOfWork, IRepairTaskRepository repairTaskRepository)
        {
            _unitOfWork = unitOfWork;
            _repairTaskRepository = repairTaskRepository;
        }

        private static RepairTaskDto MapToDto(RepairTask repairTask) => new()
        {
            Id = repairTask.Id,
            Name = repairTask.Name,
            EstimatedDuration = repairTask.EstimatedDuration ?? TimeSpan.Zero,
            DefaultLaborCost = repairTask.DefaultLaborCost,
            CreatedAt = repairTask.CreatedAt,
            UpdatedAt = repairTask.UpdatedAt
        };

        private static PartDto MapPartToDto(Part part) => new()
        {
            Id = part.Id,
            Name = part.Name,
            Description = part.Description,
            CurrentCost = part.CurrentCost,
            StockQuantity = part.StockQuantity,
            Category = part.Category,
            Supplier = part.Supplier,
            CreatedAt = part.CreatedAt,
            UpdatedAt = part.UpdatedAt
        };

        public async Task<PagedResult<RepairTaskDto>> GetAllTasks(int pageNumber, int pageSize)
        {
            var pagedTasks = await _unitOfWork.RepairTasks.GetPagedAsync(pageNumber, pageSize);
            
            var dtos = pagedTasks.Items.Select(MapToDto).ToList();
            
            return new PagedResult<RepairTaskDto>
            {
                Items = dtos,
                TotalCount = pagedTasks.TotalCount,
                PageNumber = pagedTasks.PageNumber,
                PageSize = pagedTasks.PageSize
            };
        }

        public async Task<IEnumerable<PartDto>> GetAssociatedPartsAsync(int taskId)
        {
            var parts = await _repairTaskRepository.GetAssociatedPartsAsync(taskId);
            return parts.Select(MapPartToDto).ToList();
        }

        public async Task LinkPartsAsync(int taskId, List<int> partIds)
        {
            await _repairTaskRepository.LinkPartsAsync(taskId, partIds);
        }

        public async Task<IEnumerable<RepairTask>> SearchByNameAsync(string searchTerm)
        {
            return await _repairTaskRepository.SearchByNameAsync(searchTerm);
        }
    }
}