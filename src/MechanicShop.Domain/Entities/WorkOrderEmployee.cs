using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[PrimaryKey("WorkOrderId", "EmployeeId")]
[Table("work_order_employee")]
public partial class WorkOrderEmployee
{
    [Key]
    [Column("work_order_id")]
    public int WorkOrderId { get; set; }

    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("hours_worked")]
    [Precision(5, 2)]
    public decimal? HoursWorked { get; set; }

    [Column("role")]
    [StringLength(50)]
    public string? Role { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("WorkOrderEmployees")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("WorkOrderId")]
    [InverseProperty("WorkOrderEmployees")]
    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
