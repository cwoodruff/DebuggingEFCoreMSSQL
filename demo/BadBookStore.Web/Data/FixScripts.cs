namespace BadBookStore.Web.Data;

public static class FixScripts
{
    // --- Apply “Fix V1”: computed columns for string dates & json; helpful indexes ---
    public const string V1Apply = @"
-- ===== Orders: computed datetime & source (from JSON) + indexes
IF COL_LENGTH('dbo.Orders','OrderDate_dt') IS NULL
    ALTER TABLE dbo.Orders ADD OrderDate_dt AS TRY_CONVERT(datetime2(3), OrderDate) PERSISTED;
IF COL_LENGTH('dbo.Orders','Source') IS NULL
    ALTER TABLE dbo.Orders ADD Source AS JSON_VALUE(Meta, '$.source') PERSISTED;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_CustomerEmail_OrderDate_dt')
    CREATE INDEX IX_Orders_CustomerEmail_OrderDate_dt
        ON dbo.Orders (CustomerEmail, OrderDate_dt)
        INCLUDE (OrderTotal, OrderStatus);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_Source')
    CREATE INDEX IX_Orders_Source ON dbo.Orders (Source);
GO

-- ===== OrderLines: common FK-ish lookup
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OrderLines_OrderId')
    CREATE INDEX IX_OrderLines_OrderId
        ON dbo.OrderLines (OrderId)
        INCLUDE (BookTitle, Quantity, UnitPrice, Currency);
GO

-- ===== Reviews: speed joins/filters
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reviews_ISBN')
    CREATE INDEX IX_Reviews_ISBN ON dbo.Reviews (ISBN);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reviews_CustomerEmail')
    CREATE INDEX IX_Reviews_CustomerEmail ON dbo.Reviews (CustomerEmail);
GO

-- ===== Books: title lookups (wide, but helpful)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Books_Title')
    CREATE INDEX IX_Books_Title ON dbo.Books (Title);
GO

-- ===== Inventory: seek by BookISBN (despite clustered string composite)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Inventory_BookISBN')
    CREATE INDEX IX_Inventory_BookISBN ON dbo.Inventory (BookISBN);
GO

-- ===== ActivityLog: computed datetime and index for ORDER BY
IF COL_LENGTH('dbo.ActivityLog','HappenedAt_dt') IS NULL
    ALTER TABLE dbo.ActivityLog ADD HappenedAt_dt AS TRY_CONVERT(datetime2(3), HappenedAt) PERSISTED;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ActivityLog_HappenedAt_dt_DESC')
    CREATE INDEX IX_ActivityLog_HappenedAt_dt_DESC ON dbo.ActivityLog (HappenedAt_dt DESC);
GO

-- ===== Shipments / Payments: common lookups by OrderId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Shipments_OrderId')
    CREATE INDEX IX_Shipments_OrderId ON dbo.Shipments (OrderId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Payments_OrderId')
    CREATE INDEX IX_Payments_OrderId ON dbo.Payments (OrderId);
GO
";

    // --- Rollback “Fix V1”: drop created indexes & computed columns if present ---
    public const string V1Rollback = @"
-- Drop indexes first (they depend on computed columns sometimes)
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_CustomerEmail_OrderDate_dt')
    DROP INDEX IX_Orders_CustomerEmail_OrderDate_dt ON dbo.Orders;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_Source')
    DROP INDEX IX_Orders_Source ON dbo.Orders;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OrderLines_OrderId')
    DROP INDEX IX_OrderLines_OrderId ON dbo.OrderLines;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reviews_ISBN')
    DROP INDEX IX_Reviews_ISBN ON dbo.Reviews;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reviews_CustomerEmail')
    DROP INDEX IX_Reviews_CustomerEmail ON dbo.Reviews;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Books_Title')
    DROP INDEX IX_Books_Title ON dbo.Books;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Inventory_BookISBN')
    DROP INDEX IX_Inventory_BookISBN ON dbo.Inventory;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ActivityLog_HappenedAt_dt_DESC')
    DROP INDEX IX_ActivityLog_HappenedAt_dt_DESC ON dbo.ActivityLog;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Shipments_OrderId')
    DROP INDEX IX_Shipments_OrderId ON dbo.Shipments;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Payments_OrderId')
    DROP INDEX IX_Payments_OrderId ON dbo.Payments;
GO
-- Computed columns
IF COL_LENGTH('dbo.Orders','OrderDate_dt') IS NOT NULL
    ALTER TABLE dbo.Orders DROP COLUMN OrderDate_dt;
IF COL_LENGTH('dbo.Orders','Source') IS NOT NULL
    ALTER TABLE dbo.Orders DROP COLUMN Source;
IF COL_LENGTH('dbo.ActivityLog','HappenedAt_dt') IS NOT NULL
    ALTER TABLE dbo.ActivityLog DROP COLUMN HappenedAt_dt;
GO
";

    // Optional: refresh stats to make the “after” cleaner
    public const string UpdateStats = @"
EXEC sp_updatestats;
";
}
