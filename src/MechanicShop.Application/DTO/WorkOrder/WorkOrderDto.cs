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
        public VehicleInfo Vehicle { get; init; } = null!;
        public DateTime StartAt { get; init; }
        public DateTime? EndAt { get; init; }
        public string State { get; init; } = null!; // Converted from enum to string for API
        public List<EmployeeInfo> Employees { get; init; } = new();
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record VehicleInfo
    {
        public int Id { get; init; }
        public string Make { get; init; } = null!;
        public string Model { get; init; } = null!;
        public int Year { get; init; }
        public string LicensePlate { get; init; } = null!;
        public CustomerInfo Customer { get; init; } = null!;
    }

    public record CustomerInfo
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!; // Full name from User entity
        public string Email { get; init; } = null!;
        public string? PhoneNumber { get; init; }
    }

    public record EmployeeInfo
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!; // Full name from User entity
        public string Email { get; init; } = null!;
        public string? Title { get; init; }
    }
}