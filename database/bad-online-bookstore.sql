/* ==========================================================
   DATABASE: BadBookStore (teaching anti-patterns)
   Notes:
   - Inconsistent datatypes across related tables
   - Text keys used as FKs
   - Mixed concerns, EAV, JSON blobs
   - Money as FLOAT
   - Dates as NVARCHAR
   - Sparse / missing indexes, few PKs, odd clustered choices
   ========================================================== */

-- dbo.Authors: NVARCHAR(36) GUIDs stored as text; no useful search indexes
CREATE TABLE dbo.Authors (
    AuthorId NVARCHAR(36) NOT NULL,                -- should be UNIQUEIDENTIFIER
    DisplayName NVARCHAR(400) NULL,                -- too wide
    Bio NVARCHAR(MAX) NULL,
    TwitterHandle NVARCHAR(200) NULL,              -- should be normalized or constrained
    CreatedDate NVARCHAR(30) NULL,                 -- should be DATETIME2
    CONSTRAINT PK_Authors PRIMARY KEY CLUSTERED (AuthorId) -- clustered on text GUID
);
GO

-- dbo.Books: string PK; prices as FLOAT; dates as text; status flags as 'Y'/'N'
CREATE TABLE dbo.Books (
    ISBN NVARCHAR(20) NOT NULL,                    -- string PK; different format risk
    Title NVARCHAR(500) NOT NULL,
    SubTitle NVARCHAR(500) NULL,
    AuthorId NVARCHAR(36) NULL,                    -- mismatched type to Authors.AuthorId (also text)
    ListPrice FLOAT NULL,                          -- money as FLOAT (rounding issues)
    CurrencyCode NVARCHAR(10) NULL,               
    PublishedOn NVARCHAR(30) NULL,                 -- should be DATETIME2
    CategoryCsv NVARCHAR(2000) NULL,               -- comma-separated list anti-pattern
    IsActive NVARCHAR(1) NULL,                     -- 'Y'/'N' instead of BIT
    ExtraData NVARCHAR(MAX) NULL,                  -- JSON blob with no check
    CONSTRAINT PK_Books PRIMARY KEY NONCLUSTERED (ISBN) -- nonclustered PK for no reason
);
GO

-- dbo.BookAuthors: odd bridge duplicating author on text, no FK; extra wide composite key
CREATE TABLE dbo.BookAuthors (
    BookTitle NVARCHAR(500) NOT NULL,              -- joins by Title (unstable, wide)
    AuthorDisplayName NVARCHAR(400) NOT NULL,      -- joins by name (unstable)
    Role NVARCHAR(200) NULL,
    SortOrder INT NULL,
    -- No PK/FK on purpose (duplicates possible)
);
GO

-- dbo.Customers: email as PK (unstable); phone unnormalized; dates as text
CREATE TABLE dbo.Customers (
    Email NVARCHAR(320) NOT NULL,                  -- PK as email (people change emails)
    FullName NVARCHAR(400) NULL,
    Phone NVARCHAR(100) NULL,
    BillingAddressLine NVARCHAR(1000) NULL,        -- denormalized address
    ShippingAddressLine NVARCHAR(1000) NULL,
    RegisteredOn NVARCHAR(30) NULL,                -- should be DATETIME2
    MarketingOptIn NVARCHAR(10) NULL,              -- 'yes'/'no' strings
    CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED (Email)
);
GO

-- dbo.Orders: identity PK but string FKs; totals as FLOAT; order date as text
CREATE TABLE dbo.Orders (
    OrderId BIGINT IDENTITY(1,1) NOT NULL,
    CustomerEmail NVARCHAR(320) NULL,              -- no FK to Customers
    OrderDate NVARCHAR(30) NULL,                   -- should be DATETIME2
    OrderStatus NVARCHAR(50) NULL,
    OrderTotal FLOAT NULL,                          -- money as FLOAT
    CouponCode NVARCHAR(100) NULL,
    Meta NVARCHAR(MAX) NULL,                       -- random JSON
    CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED (OrderId)
);
GO

-- dbo.OrderLines: joins to Books by Title; unit price FLOAT; missing FK
CREATE TABLE dbo.OrderLines (
    OrderLineId BIGINT IDENTITY(1,1) NOT NULL,
    OrderId BIGINT NULL,                            -- no FK to Orders
    BookTitle NVARCHAR(500) NULL,                   -- unstable join key to Books
    Quantity INT NULL,
    UnitPrice FLOAT NULL,                            -- money as FLOAT
    Currency NVARCHAR(10) NULL,
    CONSTRAINT PK_OrderLines PRIMARY KEY CLUSTERED (OrderLineId)
    -- no supporting index on (OrderId)
);
GO

-- dbo.Reviews: join by ISBN as text + also BookTitle redundancy; review date as text
CREATE TABLE dbo.Reviews (
    ReviewId BIGINT IDENTITY(1,1) NOT NULL,
    ISBN NVARCHAR(20) NULL,                         -- no FK to Books
    BookTitle NVARCHAR(500) NULL,                   -- redundant/denormalized
    CustomerEmail NVARCHAR(320) NULL,               -- no FK to Customers
    Rating INT NULL,
    ReviewText NVARCHAR(MAX) NULL,
    ReviewDate NVARCHAR(30) NULL,                   -- should be DATETIME2
    CONSTRAINT PK_Reviews PRIMARY KEY CLUSTERED (ReviewId)
);
GO

-- dbo.Inventory: composite PK w/ wide column; quantities as NVARCHAR; location as free text
CREATE TABLE dbo.Inventory (
    WarehouseCode NVARCHAR(100) NOT NULL,
    BookISBN NVARCHAR(20) NOT NULL,
    QuantityOnHand NVARCHAR(50) NULL,               -- numeric stored as text
    ReorderLevel NVARCHAR(50) NULL,                 -- numeric stored as text
    LocationNote NVARCHAR(500) NULL,
    CONSTRAINT PK_Inventory PRIMARY KEY CLUSTERED (WarehouseCode, BookISBN) -- clustered on two strings
);
GO

-- dbo.Payments: polymorphic-ish fields; card data as plain text; amount as FLOAT; date as text
CREATE TABLE dbo.Payments (
    PaymentId BIGINT IDENTITY(1,1) NOT NULL,
    OrderId BIGINT NULL,                             -- no FK to Orders
    PaidAmount FLOAT NULL,                           -- money as FLOAT
    PaidCurrency NVARCHAR(10) NULL,
    PaidOn NVARCHAR(30) NULL,                        -- should be DATETIME2
    CardLast4 NVARCHAR(10) NULL,
    CardHolder NVARCHAR(200) NULL,
    ProcessorResponse NVARCHAR(MAX) NULL,
    CONSTRAINT PK_Payments PRIMARY KEY CLUSTERED (PaymentId)
);
GO

-- dbo.Shipments: date fields as text; status free-form; address free-form again
CREATE TABLE dbo.Shipments (
    ShipmentId BIGINT IDENTITY(1,1) NOT NULL,
    OrderId BIGINT NULL,                             -- no FK to Orders
    Carrier NVARCHAR(100) NULL,
    TrackingNumber NVARCHAR(200) NULL,
    ShippedOn NVARCHAR(30) NULL,                     -- should be DATETIME2
    DeliveredOn NVARCHAR(30) NULL,                   -- should be DATETIME2
    ShipToAddressLine NVARCHAR(1000) NULL,
    Status NVARCHAR(100) NULL,
    CONSTRAINT PK_Shipments PRIMARY KEY CLUSTERED (ShipmentId)
);
GO

-- dbo.Categories: free-key name; duplicate-able; no uniqueness on Name
CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(400) NULL,                         -- not unique
    ParentCategoryName NVARCHAR(400) NULL,           -- self-ref by name, not id
    CONSTRAINT PK_Categories PRIMARY KEY CLUSTERED (CategoryId)
);
GO

-- dbo.BookCategory: bridges by strings (ISBN + Category Name), no FKs
CREATE TABLE dbo.BookCategory (
    ISBN NVARCHAR(20) NOT NULL,
    CategoryName NVARCHAR(400) NOT NULL,
    Rank INT NULL
    -- no PK; duplicates possible
);
GO

-- dbo.BookAttributes (EAV): flexible but slow; types as text; values as NVARCHAR(MAX)
CREATE TABLE dbo.BookAttributes (
    AttributeId BIGINT IDENTITY(1,1) NOT NULL,
    ISBN NVARCHAR(20) NULL,
    AttributeName NVARCHAR(200) NULL,
    AttributeType NVARCHAR(100) NULL,
    AttributeValue NVARCHAR(MAX) NULL,
    CONSTRAINT PK_BookAttributes PRIMARY KEY CLUSTERED (AttributeId)
);
GO

-- dbo.ActivityLog: catch-all audit with no structure; dates as text; who/what as blobs
CREATE TABLE dbo.ActivityLog (
    ActivityId BIGINT IDENTITY(1,1) NOT NULL,
    HappenedAt NVARCHAR(30) NULL,                    -- should be DATETIME2
    Actor NVARCHAR(500) NULL,
    Action NVARCHAR(200) NULL,
    SubjectType NVARCHAR(200) NULL,
    SubjectKey NVARCHAR(500) NULL,
    Payload NVARCHAR(MAX) NULL,
    CONSTRAINT PK_ActivityLog PRIMARY KEY CLUSTERED (ActivityId)
);
GO
