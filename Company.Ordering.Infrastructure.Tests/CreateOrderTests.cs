using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Company.Ordering.Tests;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public class CreateOrderTests: InMemoryDbTest
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
            orders[i] = new Order(default, default, "asd@example.com", default, default);
        }

        var repo = new OrdersRepository(_dbContext);

        //Act
        foreach(var order in orders)
        {
            await repo.CreateOrderAsync(order, CancellationToken.None);
        }
        await repo.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        //Assert
        foreach(var order in orders)
        {
            var orderFromDb = await _dbContext.Orders
                .SingleOrDefaultAsync(o => o.OrderNumber == order.OrderNumber, CancellationToken.None);

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
        for (var i = 0; i < amount; i++)
        {
            var order = orders[i] = new Order(default, default, "asd@example.com", default, default);
            await order.AddProductAsync(12345, default, 1, default);
            await order.AddProductAsync(12345, default, 1, default);
        }

        var repo = new OrdersRepository(_dbContext);

        //Act
        foreach (var order in orders)
        {
            await repo.CreateOrderAsync(order, CancellationToken.None);
        }
        await repo.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        //Assert
        foreach (var order in orders)
        {
            var orderFromDb = await _dbContext.Orders
                .SingleOrDefaultAsync(o => o.OrderNumber == order.OrderNumber, CancellationToken.None);

            Assert.NotNull(orderFromDb);
        }
    }
}
