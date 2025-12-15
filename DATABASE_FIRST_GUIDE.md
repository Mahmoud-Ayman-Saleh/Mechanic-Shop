# Database-First Migration Guide

This project is set up to use **Database-First** approach with an existing PostgreSQL database.

## Prerequisites

✅ All required packages are already installed:
- `Npgsql.EntityFrameworkCore.PostgreSQL` (9.0.2)
- `Microsoft.EntityFrameworkCore.Design` (9.0.0)
- `Microsoft.EntityFrameworkCore.Tools` (10.0.1)

## Steps to Scaffold Your Database

### 1. Update Connection String

Edit `src/MechanicShop.Api/appsettings.json` or `appsettings.Development.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=your_database_name;Username=your_username;Password=your_password"
  }
}
```

### 2. Option A: Use the Scaffold Script (Recommended)

```bash
# Edit the script first to add your connection string
nano scaffold-database.sh

# Run the script
./scaffold-database.sh
```

### 2. Option B: Run the Command Manually

From the project root directory, run:

```bash
cd src/MechanicShop.Infrastructure

dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5432;Database=your_database_name;Username=your_username;Password=your_password" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Entities \
  --context-dir Data \
  --context MechanicShopDbContext \
  --force \
  --no-onconfiguring \
  --data-annotations
```

### Command Options Explained

- `--output-dir Entities`: Entity classes will be created in `Entities` folder
- `--context-dir Data`: DbContext will be created in `Data` folder
- `--context MechanicShopDbContext`: Name of the DbContext class
- `--force`: Overwrite existing files
- `--no-onconfiguring`: Don't include connection string in DbContext (we'll configure it in startup)
- `--data-annotations`: Use data annotations in entities

### 3. Additional Scaffold Options (Optional)

```bash
# Scaffold specific tables only
dotnet ef dbcontext scaffold "..." Npgsql.EntityFrameworkCore.PostgreSQL \
  --table Users --table Orders --table Products

# Scaffold specific schema
dotnet ef dbcontext scaffold "..." Npgsql.EntityFrameworkCore.PostgreSQL \
  --schema public

# Use different namespace
dotnet ef dbcontext scaffold "..." Npgsql.EntityFrameworkCore.PostgreSQL \
  --namespace MechanicShop.Infrastructure.Models
```

### 4. Configure DbContext in Startup

After scaffolding, update `src/MechanicShop.Api/Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using MechanicShop.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<MechanicShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ... rest of your services

var app = builder.Build();
// ... rest of your configuration
```

### 5. Re-scaffolding After Database Changes

When your database schema changes, simply run the scaffold command again with `--force` flag to update your entities and DbContext.

## What Gets Generated

After scaffolding, you'll have:

- **Entities**: All your database tables as C# classes in `src/MechanicShop.Infrastructure/Entities/`
- **DbContext**: The main database context in `src/MechanicShop.Infrastructure/Data/MechanicShopDbContext.cs`
- **Configurations**: Entity configurations embedded in the DbContext

## Moving Entities to Domain Layer (Optional)

If you want to follow Clean Architecture principles:

1. Move generated entity classes from `Infrastructure/Entities/` to `Domain/Entities/`
2. Update namespaces
3. Keep infrastructure-specific configurations in the Infrastructure layer

## Troubleshooting

### Connection Issues
- Verify PostgreSQL is running: `sudo systemctl status postgresql`
- Test connection: `psql -h localhost -U your_username -d your_database_name`

### Permission Issues
- Ensure your database user has read permissions on all tables
- Grant necessary permissions: `GRANT SELECT ON ALL TABLES IN SCHEMA public TO your_username;`

### Schema Issues
- If tables are in a specific schema, use `--schema` option
- Default schema is `public`

## Next Steps

After scaffolding:
1. Review generated entities and DbContext
2. Add repository pattern in Infrastructure layer
3. Implement services in Application layer
4. Create API controllers in API layer
