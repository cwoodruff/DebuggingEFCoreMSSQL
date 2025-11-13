using System.Diagnostics;
using System.Text.Json;
using BadBookStore.Web.Data;
using BadBookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Pages.Admin;

public class CompareModel(BadBookStoreContext db) : PageModel
{
    [BindProperty] public List<int> Selected { get; set; } = new();

    [BindProperty] public bool KeepFixes { get; set; }

    public List<CompareResult>? Results { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Selected == null || Selected.Count == 0)
            Selected = Enumerable.Range(1, 8).ToList();

        Results = new List<CompareResult>();

        // 1) Ensure baseline: rollback first (ignore errors)
        await SafeExecAsync(FixScripts.V1Rollback);

        // 2) Baseline run
        foreach (var q in Selected)
            Results.Add(await TimeOneAsync(q));

        // 3) Apply fixes + stats
        await SafeExecAsync(FixScripts.V1Apply);
        await SafeExecAsync(FixScripts.UpdateStats);

        // 4) Fixed run
        for (int i = 0; i < Results.Count; i++)
        {
            var q = Results[i].Q;
            var fixedRes = await TimeOneAsync(q);
            Results[i] = Results[i] with
            {
                FixedMs = fixedRes.BaselineMs,
                FixedSampleJson = fixedRes.BaselineSampleJson
            };
        }

        // 5) Optional rollback to leave DB as we found it
        if (!KeepFixes)
        {
            await SafeExecAsync(FixScripts.V1Rollback);
            await SafeExecAsync(FixScripts.UpdateStats);
        }

        return Page();
    }

    private async Task<CompareResult> TimeOneAsync(int q)
    {
        var sw = Stopwatch.StartNew();
        (string Name, int count, string sampleJson, string sqlShape) = await RunQueryAsync(q);
        sw.Stop();
        return new CompareResult(
            QueryName: Name,
            Q: q,
            BaselineMs: sw.ElapsedMilliseconds,
            FixedMs: 0,
            Count: count,
            BaselineSampleJson: sampleJson,
            FixedSampleJson: "",
            SqlShape: sqlShape
        );
    }

    private async Task<(string Name, int count, string sampleJson, string sqlShape)> RunQueryAsync(int q)
    {
        switch (q)
        {
            case 1:
            {
                var start = "2023-01-01";
                var end = "2025-12-31";
                var query = db.Orders
                    .Where(o => o.CustomerEmail == "customer008@example.com"
                                && string.Compare(o.OrderDate!, start) >= 0
                                && string.Compare(o.OrderDate!, end) < 0)
                    .Select(o => new { o.OrderId, o.OrderDate, o.OrderTotal });

                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("Orders by CustomerEmail + date (string range)", count, ToJson(sample),
                    "SELECT OrderId, OrderDate, OrderTotal FROM Orders WHERE CustomerEmail='alice@example.com' AND OrderDate>='2025-01-01' AND OrderDate<'2025-12-31'");
            }

            case 2:
            {
                var query = from r in db.Reviews
                    join b in db.Books on r.BookTitle equals b.Title
                    where r.Rating >= 4
                    select new { r.ReviewId, b.Isbn, b.Title, r.Rating };
                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("Reviews ↔ Books by Title join", count, ToJson(sample),
                    "SELECT r.ReviewId, b.ISBN, b.Title, r.Rating FROM Reviews r JOIN Books b ON b.Title=r.BookTitle WHERE r.Rating>=4");
            }

            case 3:
            {
                var query = from o in db.Orders
                    join ol in db.OrderLines on o.OrderId equals ol.OrderId
                    where o.OrderStatus == "Completed"
                    select new { o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice };
                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("Orders ↔ OrderLines by OrderId (missing index)", count, ToJson(sample),
                    "SELECT o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice FROM Orders o JOIN OrderLines ol ON ol.OrderId=o.OrderId WHERE o.OrderStatus='Completed'");
            }

            case 4:
            {
                var query = db.Inventories
                    .Where(i => i.BookIsbn == "978-1-4028-0009-9")
                    .Select(i => new { i.WarehouseCode, i.BookIsbn, i.QuantityOnHand });
                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("Inventory by BookISBN (string composite key)", count, ToJson(sample),
                    "SELECT WarehouseCode, BookISBN, QuantityOnHand FROM Inventory WHERE BookISBN='978-1-4028-9462-6'");
            }

            case 5:
            {
                var category = "Programming";
                var query = db.Books
                    .Where(b =>
                        (b.CategoryCsv ?? "") == category ||
                        (b.CategoryCsv ?? "").StartsWith(category + ",") ||
                        (b.CategoryCsv ?? "").EndsWith("," + category) ||
                        (b.CategoryCsv ?? "").Contains("," + category + ","))
                    .Select(b => new { b.Isbn, b.Title, b.CategoryCsv });
                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("CategoryCsv LIKE scans", count, ToJson(sample),
                    "SELECT ISBN, Title FROM Books WHERE CategoryCsv LIKE patterns around 'Programming'");
            }

            case 6:
            {
                var query = db.ActivityLogs
                    .OrderByDescending(a => a.HappenedAt)
                    .Select(a => new { a.ActivityId, a.HappenedAt, a.Actor, a.Action });
                var count = await query.CountAsync();
                var sample = await query.Take(10).ToListAsync();
                return ("ActivityLog sort by string “date”", count, ToJson(sample),
                    "SELECT TOP(10) * FROM ActivityLog ORDER BY HappenedAt DESC");
            }

            case 7:
            {
                long demoOrderId = 12345;
                var query = db.OrderLines
                    .Where(ol => ol.OrderId == demoOrderId)
                    .Select(ol => (ol.UnitPrice ?? 0) * (ol.Quantity ?? 0));
                var sum = await query.SumAsync();
                var count = await db.OrderLines.CountAsync(ol => ol.OrderId == demoOrderId);
                return ("FLOAT money math rounding", count, ToJson(new { OrderId = demoOrderId, ComputedSum = sum }),
                    "SELECT SUM(UnitPrice * Quantity) FROM OrderLines WHERE OrderId=12345");
            }

            case 8:
            {
                var query = db.Orders
                    .Where(o => (o.Meta ?? "").Contains("\"source\":\"mobile\""))
                    .Select(o => new { o.OrderId, o.Meta });
                var count = await query.CountAsync();
                var sample = await query.Take(5).ToListAsync();
                return ("JSON-ish LIKE in Orders.Meta", count, ToJson(sample),
                    "SELECT OrderId FROM Orders WHERE Meta LIKE '%{\"source\":\"mobile\"}%'");
            }
        }

        return ("", 0, "[]", "");
    }

    private static string ToJson(object o) =>
        JsonSerializer.Serialize(o, new JsonSerializerOptions { WriteIndented = true });

    private async Task SafeExecAsync(string sql)
    {
        try
        {
            // EF can't handle GO; split on it
            var batches = System.Text.RegularExpressions.Regex
                .Split(sql, @"^\s*GO\s*$",
                    System.Text.RegularExpressions.RegexOptions.Multiline |
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrWhiteSpace(b));

            foreach (var b in batches)
                await db.Database.ExecuteSqlRawAsync(b);
        }
        catch
        {
            // swallow for demo; we always try to proceed
        }
    }

    public record CompareResult(
        int Q,
        string QueryName,
        long BaselineMs,
        long FixedMs,
        int Count,
        string BaselineSampleJson,
        string FixedSampleJson,
        string SqlShape
    );
}