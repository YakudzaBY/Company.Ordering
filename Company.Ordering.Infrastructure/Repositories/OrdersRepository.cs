using Company.Ordering.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Repositories;

public class OrdersRepository(OrderingDbContext uow)
    : Repository<Order>(uow), IOrdersRepository
{
    public async Task CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await uow.Orders.AddAsync(order, cancellationToken);
    }
}
