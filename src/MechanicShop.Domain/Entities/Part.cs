using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("part")]
public partial class Part
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("current_cost")]
    [Precision(10, 2)]
    public decimal CurrentCost { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("category")]
    [StringLength(50)]
    public string? Category { get; set; }

    [Column("supplier")]
    [StringLength(100)]
    public string? Supplier { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Part")]
    public virtual ICollection<PartPriceHistory> PartPriceHistories { get; set; } = new List<PartPriceHistory>();

    [InverseProperty("Part")]
    public virtual ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();

    [ForeignKey("PartId")]
    [InverseProperty("Parts")]
    public virtual ICollection<RepairTask> RepairTasks { get; set; } = new List<RepairTask>();
}
