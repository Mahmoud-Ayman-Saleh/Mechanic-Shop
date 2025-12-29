I'll help you create the Manager Endpoints following Clean Architecture principles. Let's start building from the domain layer up to the API layer.

## 1. Domain Layer - Repositories

First, let's create the repository interfaces in `MechanicShop.Domain/IRepository/`:

```csharp
// IRepository/IGenericRepository.cs
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<PageResult<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

```csharp
// IRepository/IRepairTaskRepository.cs
public interface IRepairTaskRepository : IGenericRepository<RepairTask>
{
    Task<IEnumerable<RepairTask>> SearchByNameAsync(string searchTerm);
    Task<IEnumerable<Part>> GetAssociatedPartsAsync(int taskId);
    Task LinkPartsAsync(int taskId, List<int> partIds);
}
```

```csharp
// IRepository/IWorkOrderRepository.cs
public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
{
    Task<WorkOrder?> GetWithDetailsAsync(int id);
    Task AssignEmployeesAsync(int workOrderId, List<int> employeeIds);
    Task AddRepairTasksAsync(int workOrderId, List<int> taskIds);
    Task AddPartsAsync(int workOrderId, List<int> partIds);
    Task<bool> ChangeStateAsync(int workOrderId, WorkOrderState newState);
}
```

```csharp
// IRepository/IInvoiceRepository.cs
public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<Invoice?> GetByWorkOrderIdAsync(int workOrderId);
    Task<Invoice> CreateFromWorkOrderAsync(int workOrderId);
}
```

```csharp
// IRepository/IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    IPartRepository Parts { get; }
    IRepairTaskRepository RepairTasks { get; }
    IWorkOrderRepository WorkOrders { get; }
    IInvoiceRepository Invoices { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

## 2. Application Layer - DTOs

```csharp
// DTO/Part/PartDto.cs
public record PartDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal CurrentCost { get; init; }
    public int StockQuantity { get; init; }
    public string? Category { get; init; }
    public string? Supplier { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
```

```csharp
// DTO/Part/CreatePartDto.cs
public record CreatePartDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal CurrentCost { get; init; }
    public int StockQuantity { get; init; }
    public string? Category { get; init; }
    public string? Supplier { get; init; }
}
```

```csharp
// DTO/Part/UpdatePartDto.cs
public record UpdatePartDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal CurrentCost { get; init; }
    public int StockQuantity { get; init; }
    public string? Category { get; init; }
    public string? Supplier { get; init; }
}
```

```csharp
// DTO/Part/AdjustStockDto.cs
public record AdjustStockDto
{
    public int Adjustment { get; init; }
}
```

```csharp
// DTO/RepairTask/RepairTaskDto.cs
public record RepairTaskDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public TimeSpan EstimatedDuration { get; init; }
    public decimal DefaultLaborCost { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
```

```csharp
// DTO/WorkOrder/CreateWorkOrderDto.cs
public record CreateWorkOrderDto
{
    public int VehicleId { get; init; }
    public List<int>? EmployeeIds { get; init; }
    public List<int>? RepairTaskIds { get; init; }
    public List<int>? PartIds { get; init; }
}
```

```csharp
// DTO/WorkOrder/WorkOrderDto.cs
public record WorkOrderDto
{
    public int Id { get; init; }
    public int VehicleId { get; init; }
    public DateTime StartAt { get; init; }
    public DateTime? EndAt { get; init; }
    public WorkOrderState State { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
```

## 3. Application Layer - Services

```csharp
// IServices/IPartService.cs
public interface IPartService
{
    Task<PageResult<PartDto>> GetAllPartsAsync(int pageNumber, int pageSize, string? category = null, string? supplier = null, string? search = null);
    Task<PartDto> CreatePartAsync(CreatePartDto dto);
    Task<PartDto> UpdatePartAsync(int partId, UpdatePartDto dto);
    Task DeletePartAsync(int partId);
    Task<PartDto> AdjustStockAsync(int partId, int adjustment);
}
```

```csharp
// IServices/IRepairTaskService.cs
public interface IRepairTaskService
{
    Task<IEnumerable<RepairTaskDto>> GetAllTasksAsync(string? search = null);
    Task<RepairTaskDto> CreateTaskAsync(CreateRepairTaskDto dto);
    Task<RepairTaskDto> UpdateTaskAsync(int taskId, UpdateRepairTaskDto dto);
    Task DeleteTaskAsync(int taskId);
    Task<IEnumerable<PartDto>> GetTaskPartsAsync(int taskId);
    Task<IEnumerable<PartDto>> LinkPartsToTaskAsync(int taskId, List<int> partIds);
}
```

```csharp
// IServices/IWorkOrderService.cs
public interface IWorkOrderService
{
    Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderDto dto);
    Task<WorkOrderDto> AssignEmployeesAsync(int workOrderId, List<int> employeeIds);
    Task<WorkOrderDto> AddRepairTasksAsync(int workOrderId, List<int> taskIds);
    Task<WorkOrderDto> AddPartsAsync(int workOrderId, List<int> partIds);
    Task<WorkOrderDto> CompleteWorkOrderAsync(int workOrderId);
}
```

```csharp
// Services/PartService.cs
public class PartService : IPartService
{
    private readonly IUnitOfWork _unitOfWork;

    public PartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PageResult<PartDto>> GetAllPartsAsync(int pageNumber, int pageSize, string? category = null, string? supplier = null, string? search = null)
    {
        var parts = await _unitOfWork.Parts.GetFilteredAsync(category, supplier, search);
        var pagedParts = parts.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        
        return new PageResult<PartDto>
        {
            Items = pagedParts.Select(MapToDto).ToList(),
            TotalCount = parts.Count(),
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PartDto> CreatePartAsync(CreatePartDto dto)
    {
        var part = new Part
        {
            Name = dto.Name,
            Description = dto.Description,
            CurrentCost = dto.CurrentCost,
            StockQuantity = dto.StockQuantity,
            Category = dto.Category,
            Supplier = dto.Supplier,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var createdPart = await _unitOfWork.Parts.AddAsync(part);
            
            // Create initial price history
            await _unitOfWork.Parts.CreatePriceHistoryAsync(createdPart.Id, dto.CurrentCost);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            return MapToDto(createdPart);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<PartDto> UpdatePartAsync(int partId, UpdatePartDto dto)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(partId);
        if (part == null)
            throw new KeyNotFoundException($"Part with ID {partId} not found.");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var costChanged = part.CurrentCost != dto.CurrentCost;
            
            part.Name = dto.Name;
            part.Description = dto.Description;
            part.CurrentCost = dto.CurrentCost;
            part.StockQuantity = dto.StockQuantity;
            part.Category = dto.Category;
            part.Supplier = dto.Supplier;
            part.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Parts.UpdateAsync(part);
            
            // Create new price history if cost changed
            if (costChanged)
                await _unitOfWork.Parts.CreatePriceHistoryAsync(partId, dto.CurrentCost);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            return MapToDto(part);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeletePartAsync(int partId)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(partId);
        if (part == null)
            throw new KeyNotFoundException($"Part with ID {partId} not found.");

        await _unitOfWork.Parts.DeleteAsync(partId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PartDto> AdjustStockAsync(int partId, int adjustment)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(partId);
        if (part == null)
            throw new KeyNotFoundException($"Part with ID {partId} not found.");

        if (part.StockQuantity + adjustment < 0)
            throw new InvalidOperationException("Adjustment would result in negative stock.");

        part.StockQuantity += adjustment;
        part.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Parts.UpdateAsync(part);
        await _unitOfWork.SaveChangesAsync();
        
        return MapToDto(part);
    }

    private static PartDto MapToDto(Part part) => new()
    {
        Id = part.Id,
        Name = part.Name,
        Description = part.Description,
        CurrentCost = part.CurrentCost,
        StockQuantity = part.StockQuantity,
        Category = part.Category,
        Supplier = part.Supplier,
        CreatedAt = part.CreatedAt,
        UpdatedAt = part.UpdatedAt
    };
}
```

## 4. Infrastructure Layer - UnitOfWork

```csharp
// Data/UnitOfWork.cs
public class UnitOfWork : IUnitOfWork
{
    private readonly MechanicShopDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(MechanicShopDbContext context, IPartRepository parts, 
        IRepairTaskRepository repairTasks, IWorkOrderRepository workOrders, 
        IInvoiceRepository invoices)
    {
        _context = context;
        Parts = parts;
        RepairTasks = repairTasks;
        WorkOrders = workOrders;
        Invoices = invoices;
    }

    public IPartRepository Parts { get; }
    public IRepairTaskRepository RepairTasks { get; }
    public IWorkOrderRepository WorkOrders { get; }
    public IInvoiceRepository Invoices { get; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
```

```csharp
// Repositories/GenericRepository.cs
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly MechanicShopDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(MechanicShopDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public virtual async Task<PageResult<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PageResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) != null;
}
```

## 5. API Layer - Controller

```csharp
// Controllers/PartsController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class PartsController : ControllerBase
{
    private readonly IPartService _partService;

    public PartsController(IPartService partService)
    {
        _partService = partService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResult<PartDto>>> GetParts(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        [FromQuery] string? supplier = null,
        [FromQuery] string? search = null)
    {
        var result = await _partService.GetAllPartsAsync(pageNumber, pageSize, category, supplier, search);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PartDto>> CreatePart([FromBody] CreatePartDto dto)
    {
        var part = await _partService.CreatePartAsync(dto);
        return CreatedAtAction(nameof(GetParts), new { id = part.Id }, part);
    }

    [HttpPut("{partId}")]
    public async Task<ActionResult<PartDto>> UpdatePart(int partId, [FromBody] UpdatePartDto dto)
    {
        try
        {
            var part = await _partService.UpdatePartAsync(partId, dto);
            return Ok(part);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{partId}")]
    public async Task<IActionResult> DeletePart(int partId)
    {
        try
        {
            await _partService.DeletePartAsync(partId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{partId}/adjust-stock")]
    public async Task<ActionResult<PartDto>> AdjustStock(int partId, [FromBody] AdjustStockDto dto)
    {
        try
        {
            var part = await _partService.AdjustStockAsync(partId, dto.Adjustment);
            return Ok(part);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

This structure follows Clean Architecture with proper separation of concerns. Continue implementing RepairTasks, WorkOrders, and Invoice controllers following the same pattern.