using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Domain.Entities;

namespace MechanicShop.Domain.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository
    {
        Task<Invoice?> GetByWorkOrderIdAsync(int workOrderId);
        Task<Invoice> CreateFromWorkOrderAsync(int workOrderId);
    }
}