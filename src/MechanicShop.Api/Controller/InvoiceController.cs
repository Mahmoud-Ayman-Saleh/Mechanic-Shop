using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechanicShop.Application.DTO.Invoice;
using MechanicShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controller
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceById(int invoiceId)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
                return Ok(invoice);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
