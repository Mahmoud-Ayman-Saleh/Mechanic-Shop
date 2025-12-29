using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicShop.Application.DTO.WorkOrder
{
    public record CreateWorkOrderDto
    {
        public int VehicleId { get; init; }
        public List<int>? EmployeeIds { get; init; }
        public List<int>? RepairTaskIds { get; init; }
        public List<int>? PartIds { get; init; }

    }
}