using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicShop.Application.DTO.Part
{
    public record AdjustStockDto
    {
        public int Adjustment { get; init; }
    }
}