using System;

namespace MechanicShop.Application.DTO.Invoice
{
    public record InvoiceDto
    {
        public int Id { get; init; }
        public int WorkOrderId { get; init; }
        public decimal Subtotal { get; init; }
        public decimal Discount { get; init; }
        public decimal TaxRate { get; init; }
        public decimal TaxAmount { get; init; }
        public decimal TotalAmount { get; init; }
        public string PaymentStatus { get; init; } = null!;
        public DateTime IssuedAt { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
