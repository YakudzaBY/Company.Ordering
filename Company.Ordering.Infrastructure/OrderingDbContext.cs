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
            .HasKey(o => o.OrderNumber);

        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .HasData(new Product
            {
                Id = 12345,
                Stock = 2
            });

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderNumber, op.ProductId });

        // Specify FK: OrderProduct.OrderNumber -> Order.OrderNumber
        modelBuilder.Entity<OrderProduct>()
            .HasOne<Order>()
            .WithMany(o => o.Products)
            .HasForeignKey(op => op.OrderNumber)
            .OnDelete(DeleteBehavior.Cascade);

        // Specify FK: OrderProduct.ProductId -> Product.Id
        modelBuilder.Entity<OrderProduct>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
