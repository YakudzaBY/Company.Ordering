using Company.Ordering.API.Models;
using Company.Ordering.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.API.Queries;

public class OrderQueries(OrderingDbContext dbContext) : IOrderQueries
{
    public async Task<OrderWithProducts?> GetOrderWithProductsAsync(int orderNumber, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Orders
            .AsNoTracking()
            .Where(o => o.Id == orderNumber)
            .Select(order => new OrderWithProducts
            {
                Products = order.Products!
                        .Select(p => new Models.OrderProduct
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            ProductPrice = p.ProductPrice,
                            ProductAmount = p.ProductAmount
                        }).ToList(),
                InvoiceAddress = order.InvoiceAddress,
                InvoiceEmailAddress = order.InvoiceEmailAddress,
                InvoiceCreditCardNumber = order.InvoiceCreditCardNumber,
                CreatedAt = order.CreatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}
