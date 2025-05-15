using Company.Ordering.API.Commands;
using Company.Ordering.Domain.Aggregates.OrderAggregate;
using MediatR;

namespace Company.Ordering.API.CommandHandlers
{
    public class CreateOrderHandler(IOrdersRepository ordersRepository)
        : IRequestHandler<CreateOrder, int>
    {
        public async Task<int> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var order = new Order(
                request.InvoiceAddress,
                request.InvoiceEmailAddress,
                request.InvoiceCreditCardNumber,
                DateTime.UtcNow);
            foreach (var product in request.Products)
            {
                await order.AddProductAsync(
                    product.ProductId,
                    product.ProductName,
                    product.ProductAmount,
                    product.ProductPrice);
            }
            await ordersRepository.CreateOrderAsync(order, cancellationToken);
            await ordersRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return order.Id;
        }
    }
}
