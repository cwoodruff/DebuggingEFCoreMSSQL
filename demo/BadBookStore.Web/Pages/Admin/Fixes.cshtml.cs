using System.Text.RegularExpressions;
using BadBookStore.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Pages.Admin;

using System.Text;

public class FixesModel(BadBookStoreContext db) : PageModel
{
    public string Message { get; private set; } = "";

    public void OnGet() {}

    public async Task<IActionResult> OnPostApplyAsync()
    {
        Message = await RunBatchAsync(FixScripts.V1Apply);
        return Page();
    }

    public async Task<IActionResult> OnPostRollbackAsync()
    {
        Message = await RunBatchAsync(FixScripts.V1Rollback);
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatsAsync()
    {
        Message = await RunBatchAsync(FixScripts.UpdateStats);
        return Page();
    }

    private async Task<string> RunBatchAsync(string sql)
    {
        var sb = new StringBuilder();
        try
        {
            // EF can't run "GO", so split batches safely.
            var batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrWhiteSpace(b));

            foreach (var batch in batches)
            {
                await db.Database.ExecuteSqlRawAsync(batch);
                sb.AppendLine("OK:");
                sb.AppendLine(batch.Length > 500 ? batch[..500] + " ... (truncated)" : batch);
                sb.AppendLine();
            }
        }
        catch (Exception ex)
        {
            sb.AppendLine("ERROR:");
            sb.AppendLine(ex.Message);
        }
        return sb.ToString();
    }
}
