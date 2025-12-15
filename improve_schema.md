```mermaid
erDiagram
    %% Users & auth
    User {
        int Id PK
        varchar(100) FirstName
        varchar(100) LastName
        varchar(100) Username
        varchar(100) Email UNIQUE
        varchar(255) PasswordHash
        varchar(20) Role
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    RefreshToken {
        int Id PK
        int UserId FK
        varchar(500) Token
        datetime ExpiresAt
        datetime CreatedAt
    }

    %% Customers & Vehicles (customers MUST have User account)
    Customer {
        int Id PK
        int UserId FK NOT NULL
        varchar(20) PhoneNumber
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    VehicleModel {
        int Id PK
        varchar(60) Make
        varchar(100) Model
        int YearFrom
        int YearTo
        varchar(50) Trim  -- optional
        boolean IsActive
    }

    Vehicle {
        int Id PK
        int CustomerId FK
        int VehicleModelId FK
        varchar(20) LicensePlate
        varchar(50) VIN
        int Year
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% Employees
    Employee {
        int Id PK
        int UserId FK
        boolean IsActive
        varchar(30) Title
        numeric salary_per_hour
        int WorkHoursPerDay DEFAULT 8
        datetime EmploymentStartDate
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% Service catalog
    RepairTask {
        int Id PK
        varchar(100) Name UNIQUE
        interval EstimatedDuration
        numeric LaborCost
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% Parts and price history
    Part {
        int Id PK
        varchar(100) SKU
        varchar(100) Name
        varchar(500) Description
        numeric CurrentCost  -- current average / last cost
        int StockQuantity
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    PartPriceHistory {
        int Id PK
        int PartId FK
        numeric UnitCost
        datetime EffectiveFrom
        datetime EffectiveTo NULL
        varchar(100) Note
        datetime CreatedAt
    }

    %% Templates linking repair tasks to parts (default quantities)
    RepairTaskPart {
        int RepairTaskId FK
        int PartId FK
        numeric DefaultQuantity
    }

    %% Overrides for specific vehicle models (variable quantities per vehicle type)
    RepairTaskPartOverride {
        int Id PK
        int RepairTaskId FK
        int PartId FK
        int VehicleModelId FK
        numeric Quantity
        varchar(200) Note
    }

    %% Work Order and its tasks
    WorkOrder {
        int Id PK
        int VehicleId FK
        int LaborId FK           -- assigned mechanic (single)
        datetime StartAt
        datetime EndAt
        enum State              -- scheduled, in_progress, completed, cancelled
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
        datetime UpdatedAt
    }

    WorkOrderRepairTask {
        int WorkOrderId FK
        int RepairTaskId FK
        numeric Quantity        -- how many times this task repeated (optional)
        varchar(200) Note      -- custom notes for this task in this order
    }

    %% Actual parts used in a specific work order (required for invoicing)
    WorkOrderPart {
        int Id PK
        int WorkOrderId FK
        int PartId FK
        numeric QuantityUsed
        numeric UnitPriceAtUse   -- snapshot price used for invoice (from PartPriceHistory)
        datetime CreatedAt
    }

    %% Invoice & payments
    Invoice {
        int Id PK
        int WorkOrderId FK UNIQUE
        numeric Subtotal
        numeric Discount
        numeric TaxRate
        numeric TaxAmount
        numeric TotalAmount
        enum PaymentStatus    -- Pending, Paid, Cancelled, Refunded
        datetime IssuedAt
        boolean IsDeleted
        datetime DeletedAt
        datetime CreatedAt
    }

    %% Relationships
    User ||--o{ Customer : "has"
    User ||--o{ Employee : "has"
    User ||--o{ RefreshToken : "tokens"

    Customer ||--o{ Vehicle : "owns"
    VehicleModel ||--o{ Vehicle : "variant of"

    Vehicle ||--o{ WorkOrder : "has work orders"
    Employee ||--o{ WorkOrder : "assigned mechanic"

    WorkOrder ||--o{ WorkOrderRepairTask : "contains"
    RepairTask ||--o{ WorkOrderRepairTask : "included in"

    RepairTask ||--o{ RepairTaskPart : "template uses parts"
    Part ||--o{ RepairTaskPart : "used as template"

    RepairTask ||--o{ RepairTaskPartOverride : "overrides"
    VehicleModel ||--o{ RepairTaskPartOverride : "for model"

    WorkOrder ||--o{ WorkOrderPart : "parts used in"
    Part ||--o{ WorkOrderPart : "used in"

    WorkOrder ||--|| Invoice : "one invoice"
    Part ||--o{ PartPriceHistory : "price history"

```