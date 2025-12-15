using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MechanicShop.Domain.Enums;

namespace MechanicShop.Domain.Entities;

[Table("invoice")]
[Index("WorkOrderId", Name = "invoice_work_order_id_key", IsUnique = true)]
public partial class Invoice
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("work_order_id")]
    public int WorkOrderId { get; set; }

    [Column("subtotal")]
    [Precision(12, 2)]
    public decimal Subtotal { get; set; }

    [Column("discount")]
    [Precision(12, 2)]
    public decimal Discount { get; set; }

    [Column("tax_rate")]
    [Precision(5, 2)]
    public decimal TaxRate { get; set; }

    [Column("tax_amount")]
    [Precision(12, 2)]
    public decimal TaxAmount { get; set; }

    [Column("total_amount")]
    [Precision(12, 2)]
    public decimal TotalAmount { get; set; }

    [Column("payment_status")]
    public PaymentStatus PaymentStatus { get; set; }

    [Column("issued_at", TypeName = "timestamp without time zone")]
    public DateTime IssuedAt { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("WorkOrderId")]
    [InverseProperty("Invoice")]
    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
