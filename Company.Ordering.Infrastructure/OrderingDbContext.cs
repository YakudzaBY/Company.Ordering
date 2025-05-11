using Company.Ordering.Domain;
using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Order> Orders { get; set; }

    internal DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>()
            .HasKey(o => o.Number);

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderNumber, op.ProductId });

        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .HasData(new Product
            {
                Id = 12345,
                Stock = 2
            });
    }
}
