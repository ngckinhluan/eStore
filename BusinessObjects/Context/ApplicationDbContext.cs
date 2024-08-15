using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
        
    }
    
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    private string? GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        return configuration.GetConnectionString("eStore");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=estore;Username=postgres;Password=12345");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary keys
        modelBuilder.Entity<Member>().HasKey(m => m.MemberId);
        modelBuilder.Entity<Member>().Property(m => m.MemberId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<Category>().Property(c => c.CategoryId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        modelBuilder.Entity<Product>().Property(p => p.ProductId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
        modelBuilder.Entity<Order>().Property(o => o.OrderId).ValueGeneratedOnAdd();
        modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductId });

        // Relationships
        modelBuilder.Entity<Product>()
            .HasOne<Category>(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne<Order>(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne<Product>(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId);

        modelBuilder.Entity<Order>()
            .HasOne<Member>(o => o.Member)
            .WithMany(m => m.Orders)
            .HasForeignKey(o => o.MemberId);

        // Sample Data
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Beverages" },
            new Category { CategoryId = 2, CategoryName = "Condiments" },
            new Category { CategoryId = 3, CategoryName = "Confections" },
            new Category { CategoryId = 4, CategoryName = "Dairy Products" },
            new Category { CategoryId = 5, CategoryName = "Grains/Cereals" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, CategoryId = 1, ProductName = "Chai", UnitPrice = 18.00f, UnitsInStock = 39 },
            new Product { ProductId = 2, CategoryId = 1, ProductName = "Chang", UnitPrice = 19.00f, UnitsInStock = 17 },
            new Product
            {
                ProductId = 3, CategoryId = 2, ProductName = "Aniseed Syrup", UnitPrice = 10.00f, UnitsInStock = 13
            },
            new Product
            {
                ProductId = 4, CategoryId = 2, ProductName = "Chef Anton's Cajun Seasoning", UnitPrice = 22.00f,
                UnitsInStock = 53
            },
            new Product
            {
                ProductId = 5, CategoryId = 2, ProductName = "Chef Anton's Gumbo Mix", UnitPrice = 21.35f,
                UnitsInStock = 0
            });

        modelBuilder.Entity<Member>().HasData(
            new Member
            {
                MemberId = 1, Email = "john.doe@example.com", CompanyName = "Doe Enterprises", City = "New York",
                Country = "USA", Password = "password123"
            },
            new Member
            {
                MemberId = 2, Email = "jane.smith@example.com", CompanyName = "Smith Co.", City = "London",
                Country = "UK", Password = "securepassword"
            }
        );
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, MemberId = 1, OrderDate = DateTime.UtcNow, RequiredDate = DateTime.UtcNow.AddDays(7), ShippedDate = DateTime.UtcNow.AddDays(2), Freight = "Standard" },
            new Order { OrderId = 2, MemberId = 2, OrderDate = DateTime.UtcNow, RequiredDate = DateTime.UtcNow.AddDays(7), ShippedDate = DateTime.UtcNow.AddDays(3), Freight = "Express" }
        );
        
        modelBuilder.Entity<OrderDetail>().HasData(
            new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 18.00f },
            new OrderDetail { OrderId = 1, ProductId = 2, Quantity = 3, UnitPrice = 19.00f },
            new OrderDetail { OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 10.00f },
            new OrderDetail { OrderId = 2, ProductId = 4, Quantity = 2, UnitPrice = 22.00f }
        );
    }
}