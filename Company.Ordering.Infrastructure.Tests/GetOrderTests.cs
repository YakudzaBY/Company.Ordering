using Company.Ordering.Infrastructure.Repositories;

namespace Company.Ordering.Infrastructure.Tests;

public class GetOrderTests : InMemoryDbTest
{
    private readonly Domain.OrderAggregate.Order _order = new()
    {
        InvoiceEmailAddress = "someone@example.com"
    };

    private readonly OrdersRepository _ordersRepository;

    public GetOrderTests(): base()
    {
        _dbContext.Orders.Add(_order);
        _dbContext.SaveChanges();

        _ordersRepository = new OrdersRepository(_dbContext);
    }

    [Fact]
    public async Task PositiveTestAsync()
    {
        //Act
        var dbOrder = await _ordersRepository.GetOrderAsync(_order.Number);

        //Assert
        Assert.NotNull(dbOrder);
    }

    [Fact]
    public async Task NegativeTestAsync()
    {
        //Act
        var dbOrder = await _ordersRepository.GetOrderAsync(-1);

        //Assert
        Assert.Null(dbOrder);
    }
}
