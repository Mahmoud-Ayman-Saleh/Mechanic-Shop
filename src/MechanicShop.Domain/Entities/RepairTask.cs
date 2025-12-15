using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("repair_task")]
[Index("Name", Name = "repair_task_name_key", IsUnique = true)]
public partial class RepairTask
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("estimated_duration")]
    public TimeSpan? EstimatedDuration { get; set; }

    [Column("default_labor_cost")]
    [Precision(10, 2)]
    public decimal DefaultLaborCost { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("RepairTask")]
    public virtual ICollection<WorkOrderRepairTask> WorkOrderRepairTasks { get; set; } = new List<WorkOrderRepairTask>();

    [ForeignKey("RepairTaskId")]
    [InverseProperty("RepairTasks")]
    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
