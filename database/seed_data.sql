-- ============================================================================
-- Mechanic Shop – Seed Data for Testing
-- Run this AFTER the DDL (mechanic_shop_ddl.pgsql) on an empty database.
-- ============================================================================

-- ======================== USERS ========================
-- Passwords are bcrypt hashes of "Password123!" (placeholder)
INSERT INTO users (first_name, last_name, username, email, password_hash, role) VALUES
('Ahmed',   'Hassan',   'ahmed.mgr',     'ahmed@mechanicshop.com',    '$2a$11$placeholder_hash_manager_1', 'Manager'),
('Sara',    'Ali',      'sara.mgr',      'sara@mechanicshop.com',     '$2a$11$placeholder_hash_manager_2', 'Manager'),
('Omar',    'Khaled',   'omar.emp',      'omar@mechanicshop.com',     '$2a$11$placeholder_hash_employee1', 'Employee'),
('Mona',    'Ibrahim',  'mona.emp',      'mona@mechanicshop.com',     '$2a$11$placeholder_hash_employee2', 'Employee'),
('Tarek',   'Saeed',    'tarek.emp',     'tarek@mechanicshop.com',    '$2a$11$placeholder_hash_employee3', 'Employee'),
('Youssef', 'Nabil',    'youssef.cust',  'youssef@email.com',         '$2a$11$placeholder_hash_customer1', 'Customer'),
('Layla',   'Mahmoud',  'layla.cust',    'layla@email.com',           '$2a$11$placeholder_hash_customer2', 'Customer'),
('Khaled',  'Fathy',    'khaled.cust',   'khaled@email.com',          '$2a$11$placeholder_hash_customer3', 'Customer');

-- ======================== CUSTOMERS ========================
-- user_id 6, 7, 8 are the Customer-role users
INSERT INTO customer (user_id, phone_number) VALUES
(6, '+20-100-111-2222'),
(7, '+20-100-333-4444'),
(8, '+20-100-555-6666');

-- ======================== EMPLOYEES ========================
-- user_id 3, 4, 5 are the Employee-role users
INSERT INTO employee (user_id, is_active, title, work_hours_per_day, employment_start_date) VALUES
(3, TRUE,  'Senior Mechanic',    8, '2022-01-15'),
(4, TRUE,  'Junior Mechanic',    8, '2023-06-01'),
(5, TRUE,  'Electrician',        8, '2023-09-10');

-- ======================== EMPLOYEE SALARY HISTORY ========================
-- Employee 1 (Omar – id=1)
INSERT INTO employee_salary_history (employee_id, hourly_rate, effective_from, effective_to) VALUES
(1, 25.00, '2022-01-15', '2024-01-01'),
(1, 30.00, '2024-01-01', NULL);

-- Employee 2 (Mona – id=2)
INSERT INTO employee_salary_history (employee_id, hourly_rate, effective_from, effective_to) VALUES
(2, 18.00, '2023-06-01', '2025-01-01'),
(2, 22.00, '2025-01-01', NULL);

-- Employee 3 (Tarek – id=3)
INSERT INTO employee_salary_history (employee_id, hourly_rate, effective_from, effective_to) VALUES
(3, 20.00, '2023-09-10', NULL);

-- ======================== VEHICLES ========================
INSERT INTO vehicle (customer_id, make, model, year, license_plate, vin) VALUES
(1, 'Toyota',     'Camry',    2020, 'ABC-1234', '1HGBH41JXMN109186'),
(1, 'Honda',      'Civic',    2019, 'DEF-5678', '2HGFC2F59MH123456'),
(2, 'BMW',        '320i',     2021, 'GHI-9012', 'WBA5R1C50KA123456'),
(2, 'Mercedes',   'C200',     2022, 'JKL-3456', 'WDDWF4KB1NR123456'),
(3, 'Hyundai',    'Elantra',  2023, 'MNO-7890', 'KMHD84LF1PU123456');

-- ======================== PARTS ========================
INSERT INTO part (name, description, current_cost, stock_quantity, category, supplier) VALUES
('Oil Filter',         'Standard oil filter for most cars',        15.50,  100, 'Filters',      'AutoParts Co.'),
('Brake Pad Set',      'Front brake pads – ceramic',               45.00,   50, 'Brakes',       'BrakeMaster'),
('Spark Plug',         'Iridium spark plug',                       12.00,  200, 'Ignition',     'SparkTech'),
('Air Filter',         'Engine air filter – universal fit',        22.00,   80, 'Filters',      'AutoParts Co.'),
('Timing Belt',        'Timing belt for 4-cylinder engines',       85.00,   30, 'Engine',       'BeltPro'),
('Coolant (1L)',       'Engine coolant – premixed',                 8.50,  150, 'Fluids',       'FluidMaster'),
('Battery 12V',        '12V car battery – 60Ah',                  120.00,   25, 'Electrical',   'PowerCell'),
('Alternator',         'Remanufactured alternator',               180.00,   10, 'Electrical',   'PowerCell'),
('Brake Disc',         'Ventilated front brake disc',              65.00,   40, 'Brakes',       'BrakeMaster'),
('Engine Oil 5W-30',   'Fully synthetic engine oil – 4L',         35.00,   60, 'Fluids',       'FluidMaster');

-- ======================== PART PRICE HISTORY ========================
INSERT INTO part_price_history (part_id, unit_cost, effective_from, effective_to) VALUES
(1, 13.00, '2023-01-01', '2024-06-01'),
(1, 15.50, '2024-06-01', NULL),
(2, 40.00, '2023-01-01', '2024-06-01'),
(2, 45.00, '2024-06-01', NULL),
(3, 10.00, '2023-01-01', NULL);

-- ======================== REPAIR TASKS ========================
INSERT INTO repair_task (name, estimated_duration, default_labor_cost) VALUES
('Oil Change',               '00:30:00', 30.00),
('Brake Pad Replacement',    '01:00:00', 60.00),
('Spark Plug Replacement',   '00:45:00', 40.00),
('Timing Belt Replacement',  '03:00:00', 150.00),
('Battery Replacement',      '00:20:00', 25.00),
('Alternator Replacement',   '02:00:00', 100.00),
('Full Inspection',          '01:30:00', 75.00),
('Coolant Flush',            '00:45:00', 45.00);

-- ======================== REPAIR TASK ↔ PART LINKS ========================
INSERT INTO repair_task_part (repair_task_id, part_id) VALUES
(1, 1),   -- Oil Change → Oil Filter
(1, 10),  -- Oil Change → Engine Oil
(2, 2),   -- Brake Pad Replacement → Brake Pads
(2, 9),   -- Brake Pad Replacement → Brake Disc
(3, 3),   -- Spark Plug Replacement → Spark Plug
(4, 5),   -- Timing Belt Replacement → Timing Belt
(5, 7),   -- Battery Replacement → Battery
(6, 8),   -- Alternator Replacement → Alternator
(8, 6);   -- Coolant Flush → Coolant

-- ======================== WORK ORDERS ========================
-- WO 1: Scheduled (ready for testing state transitions)
INSERT INTO work_order (vehicle_id, start_at, state) VALUES
(1, '2025-03-10 09:00:00', 'Scheduled');

-- WO 2: In_Progress (ready for testing the /complete endpoint)
INSERT INTO work_order (vehicle_id, start_at, state) VALUES
(2, '2025-03-11 10:00:00', 'In_Progress');

-- WO 3: In_Progress (another one for testing)
INSERT INTO work_order (vehicle_id, start_at, state) VALUES
(3, '2025-03-12 08:30:00', 'In_Progress');

-- WO 4: Completed (already has an invoice)
INSERT INTO work_order (vehicle_id, start_at, end_at, state) VALUES
(4, '2025-02-20 09:00:00', '2025-02-20 15:00:00', 'Completed');

-- WO 5: Cancelled
INSERT INTO work_order (vehicle_id, start_at, state) VALUES
(5, '2025-03-01 11:00:00', 'Cancelled');

-- ======================== WORK ORDER ↔ EMPLOYEES ========================
-- WO 1
INSERT INTO work_order_employee (work_order_id, employee_id, hours_worked, role) VALUES
(1, 1, 0, 'Lead Mechanic');

-- WO 2
INSERT INTO work_order_employee (work_order_id, employee_id, hours_worked, role) VALUES
(2, 1, 3.5, 'Lead Mechanic'),
(2, 2, 2.0, 'Assistant');

-- WO 3
INSERT INTO work_order_employee (work_order_id, employee_id, hours_worked, role) VALUES
(3, 3, 4.0, 'Electrician');

-- WO 4
INSERT INTO work_order_employee (work_order_id, employee_id, hours_worked, role) VALUES
(4, 1, 2.0, 'Lead Mechanic'),
(4, 2, 2.0, 'Assistant');

-- ======================== WORK ORDER ↔ REPAIR TASKS ========================
-- WO 2: Oil Change + Brake Pad Replacement
INSERT INTO work_order_repair_task (work_order_id, repair_task_id, quantity, labor_cost_at_use) VALUES
(2, 1, 1, 30.00),   -- Oil Change
(2, 2, 1, 60.00);   -- Brake Pad Replacement

-- WO 3: Battery Replacement + Alternator Replacement
INSERT INTO work_order_repair_task (work_order_id, repair_task_id, quantity, labor_cost_at_use) VALUES
(3, 5, 1, 25.00),   -- Battery Replacement
(3, 6, 1, 100.00);  -- Alternator Replacement

-- WO 4: Oil Change + Spark Plug Replacement
INSERT INTO work_order_repair_task (work_order_id, repair_task_id, quantity, labor_cost_at_use) VALUES
(4, 1, 1, 30.00),
(4, 3, 4, 40.00);   -- 4 spark plugs

-- ======================== WORK ORDER ↔ PARTS ========================
-- WO 2
INSERT INTO work_order_part (work_order_id, part_id, quantity_used, unit_price_at_use) VALUES
(2, 1,  1, 15.50),   -- Oil Filter x1
(2, 10, 1, 35.00),   -- Engine Oil x1
(2, 2,  1, 45.00),   -- Brake Pad Set x1
(2, 9,  2, 65.00);   -- Brake Disc x2

-- WO 3
INSERT INTO work_order_part (work_order_id, part_id, quantity_used, unit_price_at_use) VALUES
(3, 7, 1, 120.00),   -- Battery x1
(3, 8, 1, 180.00);   -- Alternator x1

-- WO 4
INSERT INTO work_order_part (work_order_id, part_id, quantity_used, unit_price_at_use) VALUES
(4, 1,  1, 15.50),   -- Oil Filter x1
(4, 10, 1, 35.00),   -- Engine Oil x1
(4, 3,  4, 12.00);   -- Spark Plug x4

-- ======================== INVOICE (for completed WO 4) ========================
-- Calculation for WO 4:
--   Parts:  15.50 + 35.00 + (4 × 12.00)  = 98.50
--   Tasks:  (1 × 30.00) + (4 × 40.00)    = 190.00
--   Labor:  Employee 1: 2h × 30.00 = 60.00 (salary effective 2024-01-01, WO started 2025-02-20)
--           Employee 2: 2h × 22.00 = 44.00 (salary effective 2025-01-01, WO started 2025-02-20)
--   Subtotal = 98.50 + 190.00 + 60.00 + 44.00 = 392.50
--   Tax (8%) = 31.40
--   Total = 423.90
INSERT INTO invoice (work_order_id, subtotal, discount, tax_rate, tax_amount, total_amount, payment_status, issued_at) VALUES
(4, 392.50, 0.00, 0.08, 31.40, 423.90, 'Paid', '2025-02-20 15:00:00');
