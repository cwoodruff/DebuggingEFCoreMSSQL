using BadBookStore.Demo.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<BadBookStoreContext>(opt =>
{
    // No migrations here on purpose; we query a pre-existing “bad” schema
    opt.UseSqlServer(builder.Configuration.GetConnectionString("BadBookStore"));
    opt.EnableSensitiveDataLogging(); // demo-friendly
});

var app = builder.Build();

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