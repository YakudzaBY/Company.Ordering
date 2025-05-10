namespace Company.Ordering.Domain;

public interface IOrdersRepository
{
    Task CreateOrderAsync(Order order);

    Task<Order> GetOrderAsync(int orderNumber);
}
