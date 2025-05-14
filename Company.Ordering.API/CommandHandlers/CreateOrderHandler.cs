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
            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                InvoiceCreditCardNumber = request.InvoiceCreditCardNumber,
                InvoiceAddress = request.InvoiceAddress,
                InvoiceEmailAddress = request.InvoiceEmailAddress,
                Products = [..request.Products
                    .Select(p => new OrderProduct
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductAmount = p.ProductAmount,
                        ProductPrice = p.ProductPrice
                    })]
            };
            await ordersRepository.CreateOrderAsync(order, cancellationToken);
            await ordersRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return order.OrderNumber;
        }
    }
}
