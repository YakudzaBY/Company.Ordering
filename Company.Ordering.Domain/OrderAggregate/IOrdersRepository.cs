namespace Company.Ordering.Domain.OrderAggregate;

public interface IOrdersRepository: IRepository<Order>
{
    Task CreateOrderAsync(Order order);

    Task<Order> GetOrderAsync(int orderNumber);
}
