using Company.Ordering.API.Commands;
using Company.Ordering.Domain.OrderAggregate;
using MediatR;

namespace Company.Ordering.API.CommandHandlers
{
    public class CreateOrderHandler(IOrdersRepository ordersRepository)
        : IRequestHandler<CreateOrder, int>
    {
        public async Task<int> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            await ordersRepository.CreateOrderAsync(request, cancellationToken);
            await ordersRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return request.Number;
        }
    }
}
