using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Application.DTO.WorkOrder;
using MechanicShop.Application.Interfaces;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Entities;
using MechanicShop.Domain.Interfaces;

namespace MechanicShop.Application.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WorkOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static WorkOrderDto MapToDto(WorkOrder workOrder) => new()
        {
            Id = workOrder.Id,
            VehicleId = workOrder.VehicleId,
            StartAt = workOrder.StartAt ?? DateTime.MinValue, // or throw exception if null
            EndAt = workOrder.EndAt,
            State = workOrder.State,
            CreatedAt = workOrder.CreatedAt,
            UpdatedAt = workOrder.UpdatedAt
        };

        
        public Task<WorkOrderDto> AddPartsAsync(int workOrderId, List<int> partIds)
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDto> AddRepairTasksAsync(int workOrderId, List<int> taskIds)
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDto> AssignEmployeesAsync(int workOrderId, List<int> employeeIds)
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDto> ChangeStateAsync(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<WorkOrderDto>> GetAllWorkOrdersAsync(int pageNumber = 1, int pageSize = 20, string? state = null, string? search = null)
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderDto> GetWorkOrderByIdAsync(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderDto>> GetWorkOrdersByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderDto>> GetWorkOrdersByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderDto>> GetWorkOrdersByVehicleIdAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }
    }
}