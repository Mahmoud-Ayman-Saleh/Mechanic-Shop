using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("part_price_history")]
public partial class PartPriceHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("part_id")]
    public int PartId { get; set; }

    [Column("unit_cost")]
    [Precision(10, 2)]
    public decimal UnitCost { get; set; }

    [Column("effective_from", TypeName = "timestamp without time zone")]
    public DateTime EffectiveFrom { get; set; }

    [Column("effective_to", TypeName = "timestamp without time zone")]
    public DateTime? EffectiveTo { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PartId")]
    [InverseProperty("PartPriceHistories")]
    public virtual Part Part { get; set; } = null!;
}
