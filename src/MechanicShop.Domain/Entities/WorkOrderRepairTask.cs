using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[PrimaryKey("WorkOrderId", "RepairTaskId")]
[Table("work_order_repair_task")]
public partial class WorkOrderRepairTask
{
    [Key]
    [Column("work_order_id")]
    public int WorkOrderId { get; set; }

    [Key]
    [Column("repair_task_id")]
    public int RepairTaskId { get; set; }

    [Column("quantity")]
    [Precision(5, 2)]
    public decimal Quantity { get; set; }

    [Column("labor_cost_at_use")]
    [Precision(10, 2)]
    public decimal LaborCostAtUse { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [ForeignKey("RepairTaskId")]
    [InverseProperty("WorkOrderRepairTasks")]
    public virtual RepairTask RepairTask { get; set; } = null!;

    [ForeignKey("WorkOrderId")]
    [InverseProperty("WorkOrderRepairTasks")]
    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
