API Design
---

## **Authentication & Authorization**

*   **POST `/api/auth/login`**
    *   **Request:**
        ```json
        {
          "username": "string",
          "password": "string"
        }
        ```
    *   **Response (200 OK):**
        ```json
        {
          "accessToken": "string",
          "refreshToken": "string",
          "user": {
            "id": 1,
            "firstName": "string",
            "lastName": "string",
            "role": "Manager|Employee|Customer"
          }
        }
        ```
    *   **Response (401 Unauthorized):** Invalid credentials.
    *   **Description:** Authenticates a user and returns JWT tokens and basic user info.

*   **POST `/api/auth/refresh-token`**
    *   **Request:**
        ```json
        {
          "refreshToken": "string"
        }
        ```
    *   **Response (200 OK):** Same as login response.
    *   **Response (401 Unauthorized):** Invalid or expired refresh token.
    *   **Description:** Refreshes the access token using a valid refresh token.

*   **POST `/api/auth/logout`**
    *   **Request:** (Include `Authorization: Bearer <token>` header)
    *   **Response (200 OK):** Empty body.
    *   **Description:** Invalidates the current refresh token.

---

## **Manager Endpoints**

### **Part Management**

*   **GET `/api/parts`**
    *   **Request:** (Optional query params: `category`, `supplier`, `search`)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "name": "Oil Filter",
            "description": "Standard oil filter for most vehicles.",
            "currentCost": 15.99,
            "stockQuantity": 50,
            "category": "Filters",
            "supplier": "AutoParts Inc.",
            "createdAt": "2023-10-27T10:00:00Z",
            "updatedAt": "2023-10-27T10:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all parts, optionally filtered.

*   **POST `/api/parts`**
    *   **Request (Body - JSON):**
        ```json
        {
          "name": "New Part Name",
          "description": "Description of the new part.",
          "currentCost": 25.50,
          "stockQuantity": 100,
          "category": "Brakes",
          "supplier": "Supplier Co."
        }
        ```
    *   **Response (201 Created):** Returns the created Part object (as in GET).
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Creates a new part record. *Note: This should also create an initial `PartPriceHistory` record with `EffectiveFrom = NOW()`.*

*   **PUT `/api/parts/{partId}`**
    *   **Request (Path Param: `partId`, Body - JSON):**
        ```json
        {
          "name": "Updated Part Name",
          "description": "Updated description.",
          "currentCost": 28.00, // Changing cost triggers PartPriceHistory creation
          "stockQuantity": 120,
          "category": "Brakes",
          "supplier": "Supplier Co."
        }
        ```
    *   **Response (200 OK):** Returns the updated Part object.
    *   **Response (404 Not Found):** Part not found.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Updates an existing part. *Critical: If `currentCost` changes, a new `PartPriceHistory` record must be created with `EffectiveFrom = NOW()`.*

*   **DELETE `/api/parts/{partId}`**
    *   **Request (Path Param: `partId`)**
    *   **Response (204 No Content):** Success.
    *   **Response (404 Not Found):** Part not found.
    *   **Description:** Soft-deletes the part (sets `IsDeleted=true`, `DeletedAt=NOW()`).

*   **PATCH `/api/parts/{partId}/adjust-stock`**
    *   **Request (Path Param: `partId`, Body - JSON):**
        ```json
        {
          "adjustment": 10 // Positive for add, Negative for remove
        }
        ```
    *   **Response (200 OK):** Returns the updated Part object.
    *   **Response (404 Not Found):** Part not found.
    *   **Response (400 Bad Request):** Adjustment would make stock negative.
    *   **Description:** Manually adjusts the `StockQuantity` of a part.

### **Repair Task Management**

*   **GET `/api/repair-tasks`**
    *   **Request:** (Optional query param: `search`)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "name": "Oil Change",
            "estimatedDuration": "PT30M", // ISO 8601 duration
            "defaultLaborCost": 45.00,
            "createdAt": "2023-10-27T10:00:00Z",
            "updatedAt": "2023-10-27T10:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all repair tasks.

*   **POST `/api/repair-tasks`**
    *   **Request (Body - JSON):**
        ```json
        {
          "name": "New Repair Task",
          "estimatedDuration": "PT1H30M",
          "defaultLaborCost": 75.00
        }
        ```
    *   **Response (201 Created):** Returns the created RepairTask object.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Creates a new repair task.

*   **PUT `/api/repair-tasks/{taskId}`**
    *   **Request (Path Param: `taskId`, Body - JSON):**
        ```json
        {
          "name": "Updated Repair Task",
          "estimatedDuration": "PT2H",
          "defaultLaborCost": 90.00
        }
        ```
    *   **Response (200 OK):** Returns the updated RepairTask object.
    *   **Response (404 Not Found):** Task not found.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Updates an existing repair task.

*   **DELETE `/api/repair-tasks/{taskId}`**
    *   **Request (Path Param: `taskId`)**
    *   **Response (204 No Content):** Success.
    *   **Response (404 Not Found):** Task not found.
    *   **Description:** Soft-deletes the repair task.

*   **GET `/api/repair-tasks/{taskId}/parts`**
    *   **Request (Path Param: `taskId`)**
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "name": "Oil Filter",
            "description": "...",
            "category": "Filters"
          },
          {
            "id": 2,
            "name": "Engine Oil",
            "description": "...",
            "category": "Fluids"
          }
        ]
        ```
    *   **Description:** Gets the list of parts typically associated with this repair task (via `RepairTaskPart`).

*   **POST `/api/repair-tasks/{taskId}/parts`**
    *   **Request (Path Param: `taskId`, Body - JSON Array of Part IDs):**
        ```json
        [1, 2]
        ```
    *   **Response (200 OK):** Returns the updated list of parts for the task (as in GET).
    *   **Response (404 Not Found):** Task or Part not found.
    *   **Description:** Links specific parts to a repair task. *Does not specify quantity; just association.*

### **Work Order Creation & Assignment**

*   **POST `/api/work-orders`**
    *   **Request (Body - JSON):**
        ```json
        {
          "vehicleId": 1,
          "employeeIds": [1, 2], // Optional, can assign later
          "repairTaskIds": [1, 3], // Optional, can add later
          "partIds": [1, 4] // Optional, can add later
        }
        ```
    *   **Response (201 Created):**
        ```json
        {
          "id": 1,
          "vehicleId": 1,
          "startAt": "2023-10-27T10:00:00Z", // Default to NOW()
          "endAt": null,
          "state": "Scheduled",
          "createdAt": "2023-10-27T10:00:00Z",
          "updatedAt": "2023-10-27T10:00:00Z"
        }
        ```
    *   **Response (400 Bad Request):** Validation errors (e.g., Vehicle not found).
    *   **Description:** Creates a new WorkOrder. *If `employeeIds`, `repairTaskIds`, or `partIds` are provided, the system automatically creates the linking records (`WorkOrderEmployee`, `WorkOrderRepairTask`, `WorkOrderPart`) and captures the `LaborCostAtUse`/`UnitPriceAtUse` at creation time.*

*   **PUT `/api/work-orders/{workOrderId}/assign-employees`**
    *   **Request (Path Param: `workOrderId`, Body - JSON Array of Employee IDs):**
        ```json
        [1, 2]
        ```
    *   **Response (200 OK):** Returns the updated WorkOrder object (including assigned employees).
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (400 Bad Request):** Employee not found or already assigned.
    *   **Description:** Assigns employees to an existing WorkOrder. *Creates `WorkOrderEmployee` records with `HoursWorked` initialized (e.g., 0).*

*   **PUT `/api/work-orders/{workOrderId}/add-repair-tasks`**
    *   **Request (Path Param: `workOrderId`, Body - JSON Array of RepairTask IDs):**
        ```json
        [1, 3]
        ```
    *   **Response (200 OK):** Returns the updated WorkOrder object (including added tasks).
    *   **Response (404 Not Found):** WorkOrder or RepairTask not found.
    *   **Response (400 Bad Request):** Task already added.
    *   **Description:** Adds repair tasks to an existing WorkOrder. *Crucially, when adding, the system captures the `DefaultLaborCost` from the `RepairTask` at that moment and stores it as `LaborCostAtUse` in the `WorkOrderRepairTask` record.*

*   **PUT `/api/work-orders/{workOrderId}/add-parts`**
    *   **Request (Path Param: `workOrderId`, Body - JSON Array of Part IDs):**
        ```json
        [1, 4]
        ```
    *   **Response (200 OK):** Returns the updated WorkOrder object (including added parts).
    *   **Response (404 Not Found):** WorkOrder or Part not found.
    *   **Response (400 Bad Request):** Part already added.
    *   **Description:** Adds parts to an existing WorkOrder. *Crucially, when adding, the system captures the `CurrentCost` from the `Part` at that moment and stores it as `UnitPriceAtUse` in the `WorkOrderPart` record. The `QuantityUsed` field is initialized to 0.*

### **Invoice Generation (Triggered by State Change)**

*   **PUT `/api/work-orders/{workOrderId}/complete`**
    *   **Request (Path Param: `workOrderId`)**
    *   **Response (200 OK):** Returns the completed WorkOrder object.
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (400 Bad Request):** WorkOrder is not in `InProgress` state.
    *   **Description:** Changes the WorkOrder state to `Completed`. *This action triggers the automatic generation of an Invoice.* The invoice calculation uses the locked `UnitPriceAtUse` and `LaborCostAtUse` values stored in the linking tables. The total labor cost is calculated as `Sum(WorkOrderEmployee.HoursWorked * EmployeeSalaryHistory.HourlyRate)` where `EmployeeSalaryHistory.EffectiveFrom <= WorkOrder.StartAt` and `EmployeeSalaryHistory.EffectiveTo > WorkOrder.StartAt` (or NULL for EffectiveTo).

*   **GET `/api/invoices/{invoiceId}`**
    *   **Request (Path Param: `invoiceId`)**
    *   **Response (200 OK):**
        ```json
        {
          "id": 1,
          "workOrderId": 1,
          "subtotal": 120.50,
          "discount": 0.00,
          "taxRate": 0.08,
          "taxAmount": 9.64,
          "totalAmount": 130.14,
          "paymentStatus": "Pending",
          "issuedAt": "2023-10-27T10:00:00Z",
          "createdAt": "2023-10-27T10:00:00Z"
        }
        ```
    *   **Response (404 Not Found):** Invoice not found.
    *   **Description:** Retrieves a specific invoice. *Managers can view any invoice.*

---

## **Employee Endpoints**

### **Work Order Management**

*   **GET `/api/employees/me/work-orders`**
    *   **Request:** (Authenticated as Employee)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "vehicleId": 1,
            "startAt": "2023-10-27T10:00:00Z",
            "endAt": null,
            "state": "Scheduled",
            "assignedEmployees": [
              { "id": 1, "hoursWorked": 0.0 },
              { "id": 2, "hoursWorked": 0.0 }
            ],
            "repairTasks": [
              { "id": 1, "name": "Oil Change", "laborCostAtUse": 45.00 }
            ],
            "parts": [
              { "id": 1, "name": "Oil Filter", "unitPriceAtUse": 15.99, "quantityUsed": 0 }
            ],
            "createdAt": "2023-10-27T10:00:00Z",
            "updatedAt": "2023-10-27T10:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all WorkOrders assigned to the currently logged-in employee. Includes details of assigned tasks and parts with their locked prices.

*   **PUT `/api/work-orders/{workOrderId}/start`**
    *   **Request (Path Param: `workOrderId`)**
    *   **Response (200 OK):** Returns the updated WorkOrder object.
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (403 Forbidden):** Employee is not assigned to this WorkOrder.
    *   **Response (400 Bad Request):** WorkOrder is not in `Scheduled` state.
    *   **Description:** Changes the WorkOrder state to `InProgress`. Only the assigned employee can perform this action.

*   **PUT `/api/work-orders/{workOrderId}/complete`**
    *   **Request (Path Param: `workOrderId`)**
    *   **Response (200 OK):** Returns the updated WorkOrder object.
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (403 Forbidden):** Employee is not assigned to this WorkOrder.
    *   **Response (400 Bad Request):** WorkOrder is not in `InProgress` state.
    *   **Description:** Changes the WorkOrder state to `Completed`. *This triggers the same invoice generation logic as the Manager's `complete` endpoint.* Only the assigned employee can perform this action.

*   **PUT `/api/work-orders/{workOrderId}/cancel`**
    *   **Request (Path Param: `workOrderId`)**
    *   **Response (200 OK):** Returns the updated WorkOrder object.
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (403 Forbidden):** Employee is not assigned to this WorkOrder.
    *   **Response (400 Bad Request):** WorkOrder is not in `InProgress` state.
    *   **Description:** Changes the WorkOrder state to `Canceled`. *Requires manager approval? (Implementation detail - might need a separate approval endpoint).*

*   **PUT `/api/work-orders/{workOrderId}/update-hours`**
    *   **Request (Path Param: `workOrderId`, Body - JSON):**
        ```json
        {
          "hoursWorked": 2.5
        }
        ```
    *   **Response (200 OK):** Returns the updated WorkOrder object (showing the updated `HoursWorked` for this employee).
    *   **Response (404 Not Found):** WorkOrder not found.
    *   **Response (403 Forbidden):** Employee is not assigned to this WorkOrder.
    *   **Response (400 Bad Request):** Invalid hours value.
    *   **Description:** Updates the `HoursWorked` for the currently logged-in employee on this WorkOrder. *This directly impacts the final invoice labor cost.*

### **Part Usage Tracking**

*   **PUT `/api/work-orders/{workOrderId}/update-part-usage`**
    *   **Request (Path Param: `workOrderId`, Body - JSON Array of Part Usage Updates):**
        ```json
        [
          {
            "partId": 1,
            "quantityUsed": 1
          },
          {
            "partId": 4,
            "quantityUsed": 2
          }
        ]
        ```
    *   **Response (200 OK):** Returns the updated WorkOrder object (showing the updated `QuantityUsed` for parts).
    *   **Response (404 Not Found):** WorkOrder or Part not found.
    *   **Response (403 Forbidden):** Employee is not assigned to this WorkOrder.
    *   **Response (400 Bad Request):** `quantityUsed` exceeds available stock or is invalid.
    *   **Description:** Updates the `QuantityUsed` for specific parts in the WorkOrder. *This action automatically decrements the `StockQuantity` in the `Part` table.*

---

## **Customer Endpoints**

### **Account Management**

*   **POST `/api/customers/register`**
    *   **Request (Body - JSON):**
        ```json
        {
          "firstName": "John",
          "lastName": "Doe",
          "username": "johndoe",
          "email": "john@example.com",
          "password": "securepassword",
          "phoneNumber": "+1234567890"
        }
        ```
    *   **Response (201 Created):** Returns the created User and Customer objects.
    *   **Response (400 Bad Request):** Validation errors or username/email already exists.
    *   **Description:** Registers a new customer account (creates User and Customer records).

*   **GET `/api/customers/me`**
    *   **Request:** (Authenticated as Customer)
    *   **Response (200 OK):**
        ```json
        {
          "id": 1,
          "userId": 1,
          "phoneNumber": "+1234567890",
          "createdAt": "2023-10-27T10:00:00Z",
          "updatedAt": "2023-10-27T10:00:00Z",
          "user": {
            "id": 1,
            "firstName": "John",
            "lastName": "Doe",
            "email": "john@example.com",
            "role": "Customer"
          }
        }
        ```
    *   **Description:** Retrieves the profile of the currently logged-in customer.

*   **PUT `/api/customers/me`**
    *   **Request (Body - JSON):**
        ```json
        {
          "phoneNumber": "+0987654321"
        }
        ```
    *   **Response (200 OK):** Returns the updated Customer object.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Updates the customer's profile information.

### **Vehicle Management**

*   **GET `/api/customers/me/vehicles`**
    *   **Request:** (Authenticated as Customer)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "customerId": 1,
            "make": "Toyota",
            "model": "Camry",
            "year": 2020,
            "licensePlate": "ABC123",
            "vin": "1HGCM82633A123456",
            "createdAt": "2023-10-27T10:00:00Z",
            "updatedAt": "2023-10-27T10:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all vehicles owned by the currently logged-in customer.

*   **POST `/api/customers/me/vehicles`**
    *   **Request (Body - JSON):**
        ```json
        {
          "make": "Honda",
          "model": "Civic",
          "year": 2022,
          "licensePlate": "XYZ789",
          "vin": "2HGCM82633A654321"
        }
        ```
    *   **Response (201 Created):** Returns the created Vehicle object.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Adds a new vehicle to the customer's account.

*   **PUT `/api/vehicles/{vehicleId}`**
    *   **Request (Path Param: `vehicleId`, Body - JSON):**
        ```json
        {
          "make": "Honda",
          "model": "Civic",
          "year": 2023,
          "licensePlate": "NEWPLATE",
          "vin": "2HGCM82633A654321"
        }
        ```
    *   **Response (200 OK):** Returns the updated Vehicle object.
    *   **Response (404 Not Found):** Vehicle not found or doesn't belong to customer.
    *   **Response (403 Forbidden):** Vehicle doesn't belong to the authenticated customer.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Updates an existing vehicle owned by the customer.

*   **DELETE `/api/vehicles/{vehicleId}`**
    *   **Request (Path Param: `vehicleId`)**
    *   **Response (204 No Content):** Success.
    *   **Response (404 Not Found):** Vehicle not found or doesn't belong to customer.
    *   **Response (403 Forbidden):** Vehicle doesn't belong to the authenticated customer.
    *   **Description:** Soft-deletes the vehicle.

### **Service Request & Tracking**

*   **POST `/api/customers/me/service-requests`**
    *   **Request (Body - JSON):**
        ```json
        {
          "vehicleId": 1,
          "description": "Car making strange noise, needs inspection."
        }
        ```
    *   **Response (201 Created):** Returns the created WorkOrder object (initially in `Scheduled` state).
    *   **Response (404 Not Found):** Vehicle not found or doesn't belong to customer.
    *   **Response (400 Bad Request):** Validation errors.
    *   **Description:** Initiates a service request, creating a new WorkOrder linked to the customer's vehicle.

*   **GET `/api/customers/me/work-orders`**
    *   **Request:** (Authenticated as Customer)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "vehicleId": 1,
            "startAt": "2023-10-27T10:00:00Z",
            "endAt": null,
            "state": "Scheduled",
            "createdAt": "2023-10-27T10:00:00Z",
            "updatedAt": "2023-10-27T10:00:00Z"
          },
          {
            "id": 2,
            "vehicleId": 1,
            "startAt": "2023-10-25T09:00:00Z",
            "endAt": "2023-10-25T11:00:00Z",
            "state": "Completed",
            "createdAt": "2023-10-25T09:00:00Z",
            "updatedAt": "2023-10-25T11:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all WorkOrders associated with the customer's vehicles.

*   **GET `/api/work-orders/{workOrderId}`**
    *   **Request (Path Param: `workOrderId`)**
    *   **Response (200 OK):** Returns the WorkOrder object.
    *   **Response (404 Not Found):** WorkOrder not found or doesn't belong to customer.
    *   **Response (403 Forbidden):** WorkOrder doesn't belong to the authenticated customer.
    *   **Description:** Retrieves details of a specific WorkOrder belonging to the customer.

*   **GET `/api/customers/me/invoices`**
    *   **Request:** (Authenticated as Customer)
    *   **Response (200 OK):**
        ```json
        [
          {
            "id": 1,
            "workOrderId": 2,
            "subtotal": 150.00,
            "discount": 10.00,
            "taxRate": 0.08,
            "taxAmount": 11.20,
            "totalAmount": 151.20,
            "paymentStatus": "Paid",
            "issuedAt": "2023-10-25T11:00:00Z",
            "createdAt": "2023-10-25T11:00:00Z"
          }
        ]
        ```
    *   **Description:** Lists all invoices for completed WorkOrders belonging to the customer.

*   **GET `/api/invoices/{invoiceId}`**
    *   **Request (Path Param: `invoiceId`)**
    *   **Response (200 OK):** Returns the specific invoice object.
    *   **Response (404 Not Found):** Invoice not found or doesn't belong to customer.
    *   **Response (403 Forbidden):** Invoice doesn't belong to the authenticated customer.
    *   **Description:** Retrieves a specific invoice belonging to the customer.

---

## **Future Considerations (Based on Requirements)**

*   **Appointment Scheduling:**
    *   `POST /api/appointments` (Customer creates appointment)
    *   `GET /api/appointments/{appointmentId}` (Customer views appointment)
    *   `PUT /api/appointments/{appointmentId}/confirm` (Manager confirms appointment, potentially creating WorkOrder)
*   **Notifications:**
    *   Webhook endpoints or internal service for sending emails/push notifications on state changes or appointment reminders.
*   **Payment Processing:**
    *   `PUT /api/invoices/{invoiceId}/pay` (Customer marks invoice as paid, updates `PaymentStatus`).
*   **Reporting:**
    *   `GET /api/reports/sales` (Manager views sales reports based on invoices).
    *   `GET /api/reports/inventory` (Manager views inventory levels and usage).
