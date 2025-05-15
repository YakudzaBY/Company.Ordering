using Company.Ordering.API.Queries;
using Company.Ordering.Tests;

namespace Company.Ordering.API.Tests;

public class GetOrderTests : InMemoryDbTest
{
    private readonly Domain.OrderAggregate.Order _order = new(default, "someone@example.com", default, default);

    private readonly OrderQueries _orderQueries;

    public GetOrderTests(): base()
    {
        _dbContext.Orders.Add(_order);
        _dbContext.SaveChanges();

        _orderQueries = new OrderQueries(_dbContext);
    }

    [Fact]
    public async Task PositiveTestAsync()
    {
        //Act
        var dbOrder = await _orderQueries.GetOrderWithProductsAsync(_order.Id);

        //Assert
        Assert.NotNull(dbOrder);
    }

    [Fact]
    public async Task NegativeTestAsync()
    {
        //Act
        var dbOrder = await _orderQueries.GetOrderWithProductsAsync(-1);

        //Assert
        Assert.Null(dbOrder);
    }
}
