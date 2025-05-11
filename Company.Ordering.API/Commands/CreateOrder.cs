using Company.Ordering.Domain.OrderAggregate;
using MediatR;

namespace Company.Ordering.API.Commands
{
    public class CreateOrder : Order, IRequest<int>
    {

    }
}
