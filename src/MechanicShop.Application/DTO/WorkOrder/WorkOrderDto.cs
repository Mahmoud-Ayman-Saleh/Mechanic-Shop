using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Enums;

namespace MechanicShop.Application.DTO.WorkOrder
{
    public record WorkOrderDto
    {
        public int Id { get; init; }
        public int VehicleId { get; init; }
        public DateTime StartAt { get; init; }
        public DateTime? EndAt { get; init; }
        public WorkOrderState State { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

    }
}