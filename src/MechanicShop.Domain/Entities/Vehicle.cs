using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("vehicle")]
public partial class Vehicle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("make")]
    [StringLength(50)]
    public string Make { get; set; } = null!;

    [Column("model")]
    [StringLength(50)]
    public string Model { get; set; } = null!;

    [Column("year")]
    public int Year { get; set; }

    [Column("license_plate")]
    [StringLength(20)]
    public string LicensePlate { get; set; } = null!;

    [Column("vin")]
    [StringLength(50)]
    public string? Vin { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Vehicles")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Vehicle")]
    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
