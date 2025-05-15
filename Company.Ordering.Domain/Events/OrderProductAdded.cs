using Company.Ordering.Domain.Aggregates.OrderAggregate;
using MediatR;

namespace Company.Ordering.Domain.Events;

public class OrderProductAdded(Order order, OrderProduct orderProduct) : INotification
{
    public Order OrderId { get; } = order;

    public OrderProduct OrderProduct { get; } = orderProduct;
}