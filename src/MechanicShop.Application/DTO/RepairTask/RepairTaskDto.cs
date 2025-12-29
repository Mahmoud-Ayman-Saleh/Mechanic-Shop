using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicShop.Application.DTO.RepairTask
{
    public record RepairTaskDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public TimeSpan EstimatedDuration { get; init; }
        public decimal DefaultLaborCost { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}