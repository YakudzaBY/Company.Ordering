using Company.Ordering.API.Queries;
using Company.Ordering.Tests;

namespace Company.Ordering.API.Tests;

public class GetOrderTests : InMemoryDbTestWithProduct
{
    private readonly Domain.OrderAggregate.Order _order = new(default, "someone@example.com", default, default);

    private readonly Func<Task<OrderQueries>> GetOrderQueriesAsync;

    public GetOrderTests(): base()
    {
        GetOrderQueriesAsync = async () =>
        {
            var dbContext = await GetDbContextAsync();
            var product = await EnsureProductAsync();

            dbContext.Orders.Add(_order);
            dbContext.SaveChanges();

            return new OrderQueries(dbContext);
        };
    }

    [Fact]
    public async Task PositiveTestAsync()
    {
        //Arrange
        var orderQueries = await GetOrderQueriesAsync();

        //Act
        var dbOrder = await orderQueries.GetOrderWithProductsAsync(_order.Id);

        //Assert
        Assert.NotNull(dbOrder);
    }

    [Fact]
    public async Task NegativeTestAsync()
    {
        //Arrange
        var orderQueries = await GetOrderQueriesAsync();

        //Act
        var dbOrder = await orderQueries.GetOrderWithProductsAsync(-1);

        //Assert
        Assert.Null(dbOrder);
    }
}
