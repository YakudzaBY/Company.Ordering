using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Company.Ordering.Tests;

namespace Company.Ordering.Infrastructure.Tests;

public class CreateOrderTests: InMemoryDbTestWithProduct
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task CreateOrderWithoutProductsAsync(int amount)
    {
        //Arrange
        var orders = new Order[amount];
        for (var i = 0; i < amount; i++)
        {
            orders[i] = new Order(default, "asd@example.com", default, default);
        }
        var dbContext = await GetDbContextAsync();

        var repo = new OrdersRepository(dbContext);

        //Act
        foreach(var order in orders)
        {
            await repo.CreateOrderAsync(order, CancellationToken.None);
        }
        await repo.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        //Assert
        foreach(var order in orders)
        {
            var orderFromDb = await dbContext.Orders
                .FindAsync(order.Id, CancellationToken.None);

            Assert.NotNull(orderFromDb);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task CreateOrderWithSameProductsAsync(int amount)
    {
        //Arrange
        var orders = new Order[amount];
        var product = await EnsureProductAsync();
        for (var i = 0; i < amount; i++)
        {
            var order = orders[i] = new Order(default, "asd@example.com", default, default);
            await order.AddProductAsync(product.Id, default, 1, default);
            await order.AddProductAsync(product.Id, default, 1, default);
        }

        var dbContext = await GetDbContextAsync();
        var repo = new OrdersRepository(dbContext);

        //Act
        foreach (var order in orders)
        {
            await repo.CreateOrderAsync(order, CancellationToken.None);
        }
        await repo.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        //Assert
        foreach (var order in orders)
        {
            var orderFromDb = await dbContext.Orders
                .FindAsync(order.Id, CancellationToken.None);

            Assert.NotNull(orderFromDb);
        }
    }
}
