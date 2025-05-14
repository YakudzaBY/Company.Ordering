using Company.Ordering.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Ordering.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderNumber);
        builder.HasMany(o => o.Products)
               .WithOne()
               .HasForeignKey(op => op.OrderNumber)
               .OnDelete(DeleteBehavior.Cascade);
    }
}