using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Entities;
using MechanicShop.Domain.Enums;

namespace MechanicShop.Domain.Interfaces
{
    public interface IWorkOrderRepository : IGenericRepository
    {
        Task<WorkOrder?> GetWithDetailsAsync(int id);
        Task AssignEmployeesAsync(int workOrderId, List<int> employeeIds);
        Task AddRepairTasksAsync(int workOrderId, List<int> taskIds);
        Task AddPartsAsync(int workOrderId, List<int> partIds);
        Task<bool> ChangeStateAsync(int workOrderId, WorkOrderState newState);
    }
}