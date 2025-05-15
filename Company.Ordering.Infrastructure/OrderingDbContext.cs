using Company.Ordering.Domain;
using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure;

public class OrderingDbContext(
    DbContextOptions<OrderingDbContext> options,
    IMediator mediator)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        await mediator.DispatchDomainEventsAsync(this, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);
    }
}
