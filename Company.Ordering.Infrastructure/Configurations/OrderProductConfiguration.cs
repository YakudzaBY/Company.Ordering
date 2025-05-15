using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Ordering.Infrastructure.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    const string OrderNumberFieldName = "OrderNumber";

    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(OrderNumberFieldName, nameof(OrderProduct.ProductId));

        builder.HasOne<Order>()
               .WithMany(o => o.Products)
               .HasForeignKey(OrderNumberFieldName)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(op => op.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}