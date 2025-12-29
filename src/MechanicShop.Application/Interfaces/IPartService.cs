using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Application.DTO.Part;
using MechanicShop.Domain.Common;

namespace MechanicShop.Application.Interfaces
{
    public interface IPartService
    {
        Task<PagedResult<PartDto>> GetAllPartsAsync(int pageNumber, int pageSize, string? category = null, string? supplier = null, string? search = null);
        
        Task<PartDto> GetPartByIdAsync(int id);
        Task<PartDto> CreatePartAsync(CreatePartDto dto);
        Task<PartDto> UpdatePartAsync(int partId, UpdatePartDto dto);
        Task DeletePartAsync(int partId);
        Task<PartDto> AdjustStockAsync(int partId, int adjustment);
    }
}