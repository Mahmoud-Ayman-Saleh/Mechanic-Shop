using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicShop.Application.DTO.Part
{
    public record UpdatePartDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public decimal CurrentCost { get; init; }
        public int StockQuantity { get; init; }
        public string? Category { get; init; }
        public string? Supplier { get; init; }

    }
}