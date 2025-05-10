using Company.Ordering.Domain;
using Company.Ordering.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>()
            .HasKey(o => o.Number);

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderNumber, op.ProductId });
    }
}
