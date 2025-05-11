using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public class CreateOrdersTests: InMemoryDbTest
{
    [Fact]
    public async Task TestAsync()
    {
        //Arrange
        var order = new Order
        {
            InvoiceEmailAddress = "asd@example.com"
        };

        var repo = new OrdersRepository(_dbContext);

        //Act
        await repo.CreateOrderAsync(order, CancellationToken.None);
        await repo.UnitOfWork.SaveChangesAsync(CancellationToken.None);

        //Assert
        var orderFromDb = await _dbContext.Orders
            .SingleOrDefaultAsync(o => o.Number == order.Number, CancellationToken.None);

        Assert.NotNull(orderFromDb);
    }
}
