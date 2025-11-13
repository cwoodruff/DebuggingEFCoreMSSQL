using System.Data;
using BadBookStore.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Pages;

public class IndexModel(BadBookStoreContext db) : PageModel
{
    public bool FixesApplied { get; private set; }

    public void OnGet()
    {
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