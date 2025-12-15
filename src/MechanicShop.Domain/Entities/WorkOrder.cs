using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MechanicShop.Domain.Enums;

namespace MechanicShop.Domain.Entities;

[Table("work_order")]
public partial class WorkOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("vehicle_id")]
    public int VehicleId { get; set; }

    [Column("start_at", TypeName = "timestamp without time zone")]
    public DateTime? StartAt { get; set; }

    [Column("end_at", TypeName = "timestamp without time zone")]
    public DateTime? EndAt { get; set; }

    [Column("state")]
    public WorkOrderState State { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("WorkOrder")]
    public virtual Invoice? Invoice { get; set; }

    [ForeignKey("VehicleId")]
    [InverseProperty("WorkOrders")]
    public virtual Vehicle Vehicle { get; set; } = null!;

    [InverseProperty("WorkOrder")]
    public virtual ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; } = new List<WorkOrderEmployee>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<WorkOrderRepairTask> WorkOrderRepairTasks { get; set; } = new List<WorkOrderRepairTask>();
}
