namespace Company.Ordering.Domain.Aggregates.OrderAggregate;

public interface IOrdersRepository: IRepository<Order>
{
    Task CreateOrderAsync(Order order, CancellationToken cancellationToken);
}
