using BadBookStore.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<BadBookStoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("BadBookStore"));

    // ---- Toggleable EF SQL logging ----
    var logSql = builder.Configuration.GetValue<bool>("EF:LogSql");
    var logSensitive = builder.Configuration.GetValue<bool>("EF:LogSensitiveData");

    if (logSql)
    {
        opt
            .EnableDetailedErrors()
            .LogTo(
                // Where to write
                Console.WriteLine,
                // What to write (only DB commands = generated SQL)
                new[] { DbLoggerCategory.Database.Command.Name },
                // Minimum level
                LogLevel.Information,
                // Include/omit scopes
                DbContextLoggerOptions.DefaultWithLocalTime);
    }

    if (logSensitive)
    {
        // Includes parameter values in logs (great for demos, not for prod)
        opt.EnableSensitiveDataLogging();
    }
});

var app = builder.Build();

// ... the rest unchanged ...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
