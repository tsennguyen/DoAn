-- Disable all constraints
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- Delete data from all tables
DELETE FROM Products;
DELETE FROM Brands;
DELETE FROM Categories;

-- Enable all constraints
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all" 