#!/bin/bash

# Database-First Scaffold Script for PostgreSQL
# This script will scaffold your existing PostgreSQL database into Entity Framework Core entities

# Navigate to Infrastructure project
cd "src/MechanicShop.Infrastructure"

# Scaffold the database
# Entities will go to Domain layer, DbContext stays in Infrastructure
dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5432;Database=mechanic-shop;Username=postgres;Password=Ma290266" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir ../MechanicShop.Domain/Entities \
  --context-dir Data \
  --context MechanicShopDbContext \
  --namespace MechanicShop.Domain.Entities \
  --context-namespace MechanicShop.Infrastructure.Data \
  --force \
  --no-onconfiguring \
  --data-annotations

echo ""
echo "Scaffolding completed!"
echo "Your entities are in: src/MechanicShop.Domain/Entities/"
echo "Your DbContext is in: src/MechanicShop.Infrastructure/Data/MechanicShopDbContext.cs"
echo ""
echo "Note: You need to manually add enum properties to entities:"
echo "  - User.Role (UserRole)"
echo "  - WorkOrder.State (WorkOrderState)"
echo "  - Invoice.PaymentStatus (PaymentStatus)"
