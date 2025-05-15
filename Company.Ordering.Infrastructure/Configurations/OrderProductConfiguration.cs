using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Ordering.Infrastructure.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasOne<Order>()
               .WithMany(o => o.Products)
               .HasForeignKey("OrderId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(op => op.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}