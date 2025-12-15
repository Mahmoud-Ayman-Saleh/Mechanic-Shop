using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("work_order_part")]
public partial class WorkOrderPart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("work_order_id")]
    public int WorkOrderId { get; set; }

    [Column("part_id")]
    public int PartId { get; set; }

    [Column("quantity_used")]
    [Precision(10, 2)]
    public decimal QuantityUsed { get; set; }

    [Column("unit_price_at_use")]
    [Precision(10, 2)]
    public decimal UnitPriceAtUse { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PartId")]
    [InverseProperty("WorkOrderParts")]
    public virtual Part Part { get; set; } = null!;

    [ForeignKey("WorkOrderId")]
    [InverseProperty("WorkOrderParts")]
    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
