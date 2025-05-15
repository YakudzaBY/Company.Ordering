using Company.Ordering.Domain.Aggregates.OrderAggregate;
using MediatR;

namespace Company.Ordering.Domain.Events;

public class OrderCreatedDomainEvent(Order order) : INotification
{

}