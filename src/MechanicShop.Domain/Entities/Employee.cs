using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Domain.Entities;

[Table("employee")]
[Index("UserId", Name = "employee_user_id_key", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("title")]
    [StringLength(50)]
    public string? Title { get; set; }

    [Column("work_hours_per_day")]
    public int WorkHoursPerDay { get; set; }

    [Column("employment_start_date")]
    public DateOnly EmploymentStartDate { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeSalaryHistory> EmployeeSalaryHistories { get; set; } = new List<EmployeeSalaryHistory>();

    [ForeignKey("UserId")]
    [InverseProperty("Employee")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; } = new List<WorkOrderEmployee>();
}
