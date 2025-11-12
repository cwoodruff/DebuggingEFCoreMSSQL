using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using BadBookStore.Web.Data;
using BadBookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Pages.Demo;

public class IndexModel(BadBookStoreContext db) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int q { get; set; } = 1;

    public bool FixesApplied { get; private set; }
    public string Title { get; private set; } = "";
    public string SqlShape { get; private set; } = "";
    public long ElapsedMs { get; private set; }
    public object? Sample { get; private set; }
    public int Count { get; private set; }

    public async Task OnGetAsync()
    {
        FixesApplied = await AreFixesAppliedAsync();
        
        var sw = Stopwatch.StartNew();

        switch (q)
        {
            case 1:
                Title = "Q1: Orders by CustomerEmail + string date range";
                // String compare + missing index on CustomerEmail
                var q1Start = "2023-01-01";
                var q1End   = "2025-12-31";
                var q1 = db.Orders
                    .Where(o => o.CustomerEmail == "customer008@example.com"
                             && string.Compare(o.OrderDate!, q1Start) >= 0
                             && string.Compare(o.OrderDate!, q1End)   <  0
                             && o.OrderStatus == "Completed")
                    .Select(o => new { o.OrderId, o.OrderDate, o.OrderTotal });

                Count = await q1.CountAsync();
                Sample = await q1.Take(15).ToListAsync();
                SqlShape = "SELECT OrderId, OrderDate, OrderTotal FROM Orders WHERE CustomerEmail = N'customer008@example.com' AND OrderDate >= N'2023-01-01' AND OrderDate < N'2025-12-31'";
                break;

            case 2:
                Title = "Q2: Reviews JOIN Books by Title (wide, non-unique)";
                var q2 = from r in db.Reviews
                         join b in db.Books on r.BookTitle equals b.Title
                         where r.Rating >= 4
                         select new { r.ReviewId, b.Isbn, b.Title, r.Rating };

                Count = await q2.CountAsync();
                Sample = await q2.Take(15).ToListAsync();
                SqlShape = "SELECT r.ReviewId, b.ISBN, b.Title, r.Rating FROM Reviews r JOIN Books b ON b.Title = r.BookTitle WHERE r.Rating >= 4";
                break;

            case 3:
                Title = "Q3: Orders ↔ OrderLines (missing IX on OrderLines(OrderId))";
                var q3 = from o in db.Orders
                         join ol in db.OrderLines on o.OrderId equals ol.OrderId
                         where o.OrderStatus == "Completed"
                         select new { o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice };

                Count = await q3.CountAsync();
                Sample = await q3.Take(15).ToListAsync();
                SqlShape = "SELECT o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice FROM Orders o JOIN OrderLines ol ON ol.OrderId = o.OrderId WHERE o.OrderStatus = N'Completed'";
                break;

            case 4:
                Title = "Q4: Inventory by BookISBN (string composite clustered key)";
                var q4 = db.Inventories
                    .Where(i => i.BookIsbn == "978-1-4028-0009-9")
                    .Select(i => new { i.WarehouseCode, i.BookIsbn, i.QuantityOnHand });

                Count = await q4.CountAsync();
                Sample = await q4.Take(15).ToListAsync();
                SqlShape = "SELECT WarehouseCode, BookISBN, QuantityOnHand FROM Inventory WHERE BookISBN = N'978-1-4028-0009-9'";
                break;

            case 5:
                Title = "Q5: CategoryCsv LIKE scans";
                var category = "Programming";
                var q5 = db.Books
                    .Where(b =>
                        (b.CategoryCsv ?? "") == category ||
                        (b.CategoryCsv ?? "").StartsWith(category + ",") ||
                        (b.CategoryCsv ?? "").EndsWith("," + category) ||
                        (b.CategoryCsv ?? "").Contains("," + category + ","))
                    .Select<Book, object>(b => new { b.Isbn, b.Title, b.CategoryCsv });

                Count = await q5.CountAsync();
                Sample = await q5.Take(15).ToListAsync();
                SqlShape = "SELECT ISBN, Title FROM Books WHERE CategoryCsv LIKE patterns around 'Programming'";
                break;

            case 6:
                Title = "Q6: ActivityLog sorted by nvarchar 'date'";
                var q6 = db.ActivityLogs
                    .OrderByDescending(a => a.HappenedAt)
                    .Select(a => new { a.ActivityId, a.HappenedAt, a.Actor, a.Action });

                Count = await q6.CountAsync();
                Sample = await q6.Take(15).ToListAsync();
                SqlShape = "SELECT TOP(15) * FROM ActivityLog ORDER BY HappenedAt DESC";
                break;

            case 7:
                Title = "Q7: FLOAT money math (rounding differences)";
                long demoOrderId = 12345; // change to an existing OrderId in your DB
                var q7 = db.OrderLines
                    .Where(ol => ol.OrderId == demoOrderId)
                    .Select(ol => (ol.UnitPrice ?? 0) * (ol.Quantity ?? 0));

                var sum = await q7.SumAsync();
                Count = await db.OrderLines.CountAsync(ol => ol.OrderId == demoOrderId);
                Sample = new { OrderId = demoOrderId, ComputedSum = sum };
                SqlShape = "SELECT SUM(UnitPrice * Quantity) FROM OrderLines WHERE OrderId = @p0";
                break;

            case 8:
                Title = "Q8: JSON-ish LIKE in Orders.Meta (full scans)";
                var q8 = db.Orders
                    .Where(o => (o.Meta ?? "").Contains("\"source\":\"mobile\""))
                    .Select(o => new { o.OrderId, o.Meta });

                Count = await q8.CountAsync();
                Sample = await q8.Take(15).ToListAsync();
                SqlShape = "SELECT OrderId FROM Orders WHERE Meta LIKE N'%{\"source\":\"mobile\"}%'";
                break;

            default:
                Title = "Choose a query from the home page.";
                break;
        }

        sw.Stop();
        ElapsedMs = sw.ElapsedMilliseconds;
    }
    
    private async Task<bool> AreFixesAppliedAsync()
    {
        // We consider “fixes applied” if the computed column OrderDate_dt exists.
        const string sql = @"
            SELECT CASE WHEN EXISTS (
                SELECT 1
                FROM sys.columns
                WHERE object_id = OBJECT_ID(N'dbo.Orders')
                  AND name = N'OrderDate_dt'
            ) THEN 1 ELSE 0 END";

        var conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var result = await cmd.ExecuteScalarAsync();
        return result is int i && i == 1;
    }
}
