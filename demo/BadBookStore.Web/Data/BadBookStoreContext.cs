using System;
using System.Collections.Generic;
using BadBookStore.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BadBookStore.Web.Data;

public partial class BadBookStoreContext : DbContext
{
    public BadBookStoreContext()
    {
    }

    public BadBookStoreContext(DbContextOptions<BadBookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAttribute> BookAttributes { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("ActivityLog");

            entity.Property(e => e.Action).HasMaxLength(200);
            entity.Property(e => e.Actor).HasMaxLength(500);
            entity.Property(e => e.HappenedAt).HasMaxLength(30);
            entity.Property(e => e.SubjectKey).HasMaxLength(500);
            entity.Property(e => e.SubjectType).HasMaxLength(200);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.AuthorId).HasMaxLength(36);
            entity.Property(e => e.CreatedDate).HasMaxLength(30);
            entity.Property(e => e.DisplayName).HasMaxLength(400);
            entity.Property(e => e.TwitterHandle).HasMaxLength(200);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).IsClustered(false);

            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.AuthorId).HasMaxLength(36);
            entity.Property(e => e.CategoryCsv).HasMaxLength(2000);
            entity.Property(e => e.CurrencyCode).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasMaxLength(1);
            entity.Property(e => e.PublishedOn).HasMaxLength(30);
            entity.Property(e => e.SubTitle).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<BookAttribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId);

            entity.Property(e => e.AttributeName).HasMaxLength(200);
            entity.Property(e => e.AttributeType).HasMaxLength(100);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AuthorDisplayName).HasMaxLength(400);
            entity.Property(e => e.BookTitle).HasMaxLength(500);
            entity.Property(e => e.Role).HasMaxLength(200);
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BookCategory");

            entity.Property(e => e.CategoryName).HasMaxLength(400);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(400);
            entity.Property(e => e.ParentCategoryName).HasMaxLength(400);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.BillingAddressLine).HasMaxLength(1000);
            entity.Property(e => e.FullName).HasMaxLength(400);
            entity.Property(e => e.MarketingOptIn).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.RegisteredOn).HasMaxLength(30);
            entity.Property(e => e.ShippingAddressLine).HasMaxLength(1000);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseCode, e.BookIsbn });

            entity.ToTable("Inventory");

            entity.Property(e => e.WarehouseCode).HasMaxLength(100);
            entity.Property(e => e.BookIsbn)
                .HasMaxLength(20)
                .HasColumnName("BookISBN");
            entity.Property(e => e.LocationNote).HasMaxLength(500);
            entity.Property(e => e.QuantityOnHand).HasMaxLength(50);
            entity.Property(e => e.ReorderLevel).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.CouponCode).HasMaxLength(100);
            entity.Property(e => e.CustomerEmail).HasMaxLength(320);
            entity.Property(e => e.OrderDate).HasMaxLength(30);
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.Property(e => e.BookTitle).HasMaxLength(500);
            entity.Property(e => e.Currency).HasMaxLength(10);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.CardHolder).HasMaxLength(200);
            entity.Property(e => e.CardLast4).HasMaxLength(10);
            entity.Property(e => e.PaidCurrency).HasMaxLength(10);
            entity.Property(e => e.PaidOn).HasMaxLength(30);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.BookTitle).HasMaxLength(500);
            entity.Property(e => e.CustomerEmail).HasMaxLength(320);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.ReviewDate).HasMaxLength(30);
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.Property(e => e.Carrier).HasMaxLength(100);
            entity.Property(e => e.DeliveredOn).HasMaxLength(30);
            entity.Property(e => e.ShipToAddressLine).HasMaxLength(1000);
            entity.Property(e => e.ShippedOn).HasMaxLength(30);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TrackingNumber).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}