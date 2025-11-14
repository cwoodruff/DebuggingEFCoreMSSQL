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
    [BindProperty(SupportsGet = true)] public int q { get; set; } = 1;

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
                var q1End = "2025-12-31";
                var q1 = db.Orders
                    .Where(o => o.CustomerEmail == "customer008@example.com"
                                && string.Compare(o.OrderDate!, q1Start) >= 0
                                && string.Compare(o.OrderDate!, q1End) < 0
                                && o.OrderStatus == "Completed")
                    .Select(o => new { o.OrderId, o.OrderDate, o.OrderTotal });

                Count = await q1.CountAsync();
                Sample = await q1.Take(15).ToListAsync();
                SqlShape =
                    "SELECT OrderId, OrderDate, OrderTotal FROM Orders WHERE CustomerEmail = N'customer008@example.com' AND OrderDate >= N'2023-01-01' AND OrderDate < N'2025-12-31' AND o.OrderStatus = 'Completed'";
                break;

            case 2:
                Title = "Q2: Reviews JOIN Books by Title (wide, non-unique)";
                var q2 = from r in db.Reviews
                    join b in db.Books on r.BookTitle equals b.Title
                    where r.Rating >= 4
                    select new { r.ReviewId, b.Isbn, b.Title, r.Rating };

                Count = await q2.CountAsync();
                Sample = await q2.Take(15).ToListAsync();
                SqlShape =
                    "SELECT r.ReviewId, b.ISBN, b.Title, r.Rating FROM Reviews r JOIN Books b ON b.Title = r.BookTitle WHERE r.Rating >= 4";
                break;

            case 3:
                Title = "Q3: Orders ↔ OrderLines (missing IX on OrderLines(OrderId))";
                var q3 = from o in db.Orders
                    join ol in db.OrderLines on o.OrderId equals ol.OrderId
                    where o.OrderStatus == "Completed"
                    select new { o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice };

                Count = await q3.CountAsync();
                Sample = await q3.Take(15).ToListAsync();
                SqlShape =
                    "SELECT o.OrderId, ol.BookTitle, ol.Quantity, ol.UnitPrice FROM Orders o JOIN OrderLines ol ON ol.OrderId = o.OrderId WHERE o.OrderStatus = N'Completed'";
                break;

            case 4:
                Title = "Q4: Inventory by BookISBN (string composite clustered key)";
                var q4 = db.Inventories
                    .Where(i => i.BookIsbn == "978-1-4028-0009-9")
                    .Select(i => new { i.WarehouseCode, i.BookIsbn, i.QuantityOnHand });

                Count = await q4.CountAsync();
                Sample = await q4.Take(15).ToListAsync();
                SqlShape =
                    "SELECT WarehouseCode, BookISBN, QuantityOnHand FROM Inventory WHERE BookISBN = N'978-1-4028-0009-9'";
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
                SqlShape = "SELECT ISBN, Title FROM Books WHERE CategoryCsv LIKE 'Programming'";
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
    
    public IActionResult OnGetExplain(int q)
    {
        if (q == 0) return Content(string.Empty, "text/html");
        var html = GetExplainHtml(q);
        return Content(html, "text/html");
    }
    
    private static string GetExplainHtml(int q) => q switch
    {
        1 => Block("Q1 — Orders by CustomerEmail + string date range", """
            <p><strong>Demonstrates</strong> String dates (<code>NVARCHAR</code>) kill sargability; missing composite index.</p>
            <p><strong>Query shape:</strong> <code>WHERE CustomerEmail='customer008@example.com' AND OrderDate &gt;= '2023-01-01' AND OrderDate &lt; '2025-12-31'</code></p>
            <p><strong>Why slow:</strong> string compares + no (CustomerEmail, OrderDate) index =&gt; scans.</p>
            <p><strong>Plan tells:</strong> clustered/index scan; high logical reads.</p>
            <p><strong>Fix V1 effect:</strong> adds persisted <code>OrderDate_dt</code> + <code>IX_Orders_CustomerEmail_OrderDate_dt</code> =&gt; seeks.</p>
            <p><strong>Real fix:</strong> store <code>datetime2</code>; index <code>(CustomerEmail, OrderDate)</code> with INCLUDEs.</p>
        """),
        2 => Block("Q2 — Reviews ↔ Books join by Title", """
            <p><strong>Demonstrates</strong> Joining on non-unique, wide text; missing title index.</p>
            <p><strong>Query shape:</strong> <code>JOIN Books b ON b.Title = r.BookTitle WHERE r.Rating &gt;= 4</code></p>
            <p><strong>Why slow:</strong> wide/duplicate join key; no <code>Books(Title)</code> index =&gt; scans/hash.</p>
            <p><strong>Plan tells:</strong> hash join, large memory grant; possible spills.</p>
            <p><strong>Fix V1 effect:</strong> <code>IX_Books_Title</code> helps probes but design still weak.</p>
            <p><strong>Real fix:</strong> join on stable key (<code>BookId</code>), enforce FKs.</p>
        """),
        3 => Block("Q3 — Orders ↔ OrderLines (missing child index)", """
            <p><strong>Demonstrates</strong> No index on FK-like column <code>OrderLines(OrderId)</code>.</p>
            <p><strong>Query shape:</strong> <code>JOIN OrderLines ol ON ol.OrderId = o.OrderId WHERE o.OrderStatus='Completed'</code></p>
            <p><strong>Why slow:</strong> N+1 joins/scans on lines.</p>
            <p><strong>Plan tells:</strong> nested loops + repeated scans / or hash with full scans.</p>
            <p><strong>Fix V1 effect:</strong> <code>IX_OrderLines_OrderId</code> (covering) =&gt; seek + far fewer reads.</p>
            <p><strong>Real fix:</strong> always index FKs; consider composite covering indexes.</p>
        """),
        4 => Block("Q4 — Inventory by BookISBN under string composite clustered key", """
            <p><strong>Demonstrates</strong> Bad clustering: <code>(WarehouseCode, BookISBN)</code> NVARCHAR strings.</p>
            <p><strong>Query shape:</strong> <code>WHERE BookISBN='978-1-4028-0009-9'</code></p>
            <p><strong>Why slow:</strong> predicate on second key; no helper index =&gt; scans.</p>
            <p><strong>Plan tells:</strong> clustered scan with residual predicate.</p>
            <p><strong>Fix V1 effect:</strong> <code>IX_Inventory_BookISBN</code> =&gt; direct seek.</p>
            <p><strong>Real fix:</strong> narrow clustered key (surrogate) + targeted secondaries.</p>
        """),
        5 => Block("Q5 — CategoryCsv LIKE scans (CSV anti-pattern)", """
            <p><strong>Demonstrates</strong> CSV in a column makes membership tests unsargable.</p>
            <p><strong>Query shape:</strong> boundary <code>LIKE</code> 'Programming'</p>
            <p><strong>Why slow:</strong> engine can’t reason about sets in CSV; scans even with indexes.</p>
            <p><strong>Fix V1 effect:</strong> none (on purpose) — shows limits of indexing.</p>
            <p><strong>Real fix:</strong> normalize via <code>BookCategory</code> bridge and join.</p>
        """),
        6 => Block("Q6 — ActivityLog ordered by string ‘date’", """
            <p><strong>Demonstrates</strong> Sorting by text date; no ordered access path.</p>
            <p><strong>Query shape:</strong> <code>TOP(10) ... ORDER BY HappenedAt DESC</code></p>
            <p><strong>Why slow:</strong> full sort; possible tempdb spills.</p>
            <p><strong>Fix V1 effect:</strong> computed <code>HappenedAt_dt</code> + DESC index =&gt; ordered seek.</p>
            <p><strong>Real fix:</strong> store <code>datetime2</code>; align indexes to read patterns.</p>
        """),
        7 => Block("Q7 — FLOAT money math (correctness, not speed)", """
            <p><strong>Demonstrates</strong> <code>FLOAT</code> is imprecise for currency; totals vary.</p>
            <p><strong>Query shape:</strong> <code>SUM(UnitPrice * Quantity) WHERE OrderId=12345</code></p>
            <p><strong>Why bad:</strong> binary floating-point can’t represent many decimal fractions.</p>
            <p><strong>Fix V1 effect:</strong> none — indexing can’t fix wrong math.</p>
            <p><strong>Real fix:</strong> use <code>DECIMAL(19,4)</code> for money everywhere.</p>
        """),
        8 => Block("Q8 — JSON-ish LIKE probe in Orders.Meta", """
            <p><strong>Demonstrates</strong> NVARCHAR(MAX) + leading <code>%LIKE%</code> forces scans.</p>
            <p><strong>Query shape:</strong> <code>WHERE Meta LIKE '%{"}source{"}:"}mobile{"}%'</code></p>
            <p><strong>Why slow:</strong> no stats on JSON attributes; large LOB scans.</p>
            <p><strong>Fix V1 effect:</strong> persisted <code>Source = JSON_VALUE(Meta,'$.source')</code> + index; filter by <code>Source='mobile'</code>.</p>
            <p><strong>Real fix:</strong> model important attributes as columns or computed projections.</p>
        """),
        _ => "<p>No details found.</p>"
    };

    private static string Block(string title, string body) =>
$"""
<h3 style="margin:0 0 .5rem 0">{title}</h3>
{body}
""";
}