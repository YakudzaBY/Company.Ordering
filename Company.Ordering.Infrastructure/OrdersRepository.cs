using Company.Ordering.Domain;

namespace Company.Ordering.Infrastructure;

public class OrdersRepository : IOrdersRepository
{
    public async Task CreateOrderAsync(Order order)
    {
        //TODO
    }

    public async Task<Order> GetOrderAsync(int orderNumber)
    {
        //UNDONE
        return new Order
        {
            Number = orderNumber,
        };
    }
}
