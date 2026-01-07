using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicShop.Application.DTO.RepairTask
{
    public record CreateRepairTaskDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Estimated duration is required")]
        public TimeSpan EstimatedDuration { get; init; }

        [Required(ErrorMessage = "Default labor cost is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Default labor cost must be greater than 0")]
        public decimal DefaultLaborCost { get; init; }
    }
}
