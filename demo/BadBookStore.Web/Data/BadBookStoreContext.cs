using BadBookStore.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Data;

public class BadBookStoreContext(DbContextOptions<BadBookStoreContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>(); // keyless
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Inventory> Inventory => Set<Inventory>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>(); // keyless
    public DbSet<BookAttribute> BookAttributes => Set<BookAttribute>();
    public DbSet<ActivityLog> ActivityLog => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Author>().ToTable("Authors").HasKey(x => x.AuthorId);
        mb.Entity<Author>().Property(x => x.AuthorId).HasColumnType("nvarchar(36)");

        mb.Entity<Book>().ToTable("Books").HasKey(x => x.ISBN);
        mb.Entity<Book>().Property(x => x.ISBN).HasColumnType("nvarchar(20)");

        mb.Entity<BookAuthor>().ToTable("BookAuthors").HasNoKey(); // table has no PK
        mb.Entity<BookAuthor>().Property(x => x.BookTitle).HasColumnType("nvarchar(500)");

        mb.Entity<Customer>().ToTable("Customers").HasKey(x => x.Email);
        mb.Entity<Customer>().Property(x => x.Email).HasColumnType("nvarchar(320)");

        mb.Entity<Order>().ToTable("Orders").HasKey(x => x.OrderId);
        mb.Entity<Order>().Property(x => x.CustomerEmail).HasColumnType("nvarchar(320)");

        mb.Entity<OrderLine>().ToTable("OrderLines").HasKey(x => x.OrderLineId);
        mb.Entity<OrderLine>().Property(x => x.BookTitle).HasColumnType("nvarchar(500)");

        mb.Entity<Review>().ToTable("Reviews").HasKey(x => x.ReviewId);
        mb.Entity<Review>().Property(x => x.ISBN).HasColumnType("nvarchar(20)");

        mb.Entity<Inventory>().ToTable("Inventory").HasKey(x => new { x.WarehouseCode, x.BookISBN });
        mb.Entity<Inventory>().Property(x => x.WarehouseCode).HasColumnType("nvarchar(100)");
        mb.Entity<Inventory>().Property(x => x.BookISBN).HasColumnType("nvarchar(20)");

        mb.Entity<Payment>().ToTable("Payments").HasKey(x => x.PaymentId);
        mb.Entity<Shipment>().ToTable("Shipments").HasKey(x => x.ShipmentId);

        mb.Entity<Category>().ToTable("Categories").HasKey(x => x.CategoryId);

        mb.Entity<BookCategory>().ToTable("BookCategory").HasNoKey(); // table has no PK
        mb.Entity<BookCategory>().Property(x => x.ISBN).HasColumnType("nvarchar(20)");

        mb.Entity<BookAttribute>().ToTable("BookAttributes").HasKey(x => x.AttributeId);
        mb.Entity<ActivityLog>().ToTable("ActivityLog").HasKey(x => x.ActivityId);

        // No FKs on purpose to mirror the broken schema
    }
}
