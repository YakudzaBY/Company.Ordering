using Company.Ordering.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Repositories;

public class OrdersRepository(OrderingDbContext uow)
    : Repository<Order>(uow), IOrdersRepository
{
    public async Task CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        order.Guid = Guid.NewGuid();
        await uow.Orders.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetOrderWithProductsAsync(int orderNumber, CancellationToken cancellationToken = default)
    {
        return await uow
            .Orders
            .AsNoTracking()
            .Include(o => o.Products)
            .SingleOrDefaultAsync(o => o.Number == orderNumber, cancellationToken);
    }

    public async Task<Order?> GetOrderWithProductsAsync(Guid orderNumber, CancellationToken cancellationToken = default)
    {
        return await uow
            .Orders
            .AsNoTracking()
            .Include(o => o.Products)
            .SingleOrDefaultAsync(o => o.Guid == orderNumber, cancellationToken);
    }
}
