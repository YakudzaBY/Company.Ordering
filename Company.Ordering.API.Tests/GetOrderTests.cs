using Company.Ordering.API.Queries;
using Company.Ordering.Tests;

namespace Company.Ordering.API.Tests;

public class GetOrderTests : InMemoryDbTest
{
    private readonly Domain.OrderAggregate.Order _order = new(default, default, "someone@example.com", default, default);

    private readonly OrderQueries _ordersRepository;

    public GetOrderTests(): base()
    {
        _dbContext.Orders.Add(_order);
        _dbContext.SaveChanges();

        _ordersRepository = new OrderQueries(_dbContext);
    }

    [Fact]
    public async Task PositiveTestAsync()
    {
        //Act
        var dbOrder = await _ordersRepository.GetOrderWithProductsAsync(_order.OrderNumber);

        //Assert
        Assert.NotNull(dbOrder);
    }

    [Fact]
    public async Task NegativeTestAsync()
    {
        //Act
        var dbOrder = await _ordersRepository.GetOrderWithProductsAsync(-1);

        //Assert
        Assert.Null(dbOrder);
    }
}
