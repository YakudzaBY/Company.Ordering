using Company.Ordering.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Repositories;

public class OrdersRepository(OrderingDbContext uow)
    : Repository<Order>(uow), IOrdersRepository
{
    public async Task CreateOrderAsync(Order order)
    {
        await uow.Orders.AddAsync(order);
    }

    public async Task<Order?> GetOrderAsync(int orderNumber)
    {
        return await uow
            .Orders
            .Include(o => o.Products)
            .SingleOrDefaultAsync(o => o.Number == orderNumber);
    }
}
