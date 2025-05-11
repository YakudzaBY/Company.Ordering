using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public class CreateOrderTests: InMemoryDbTest
{
    [Theory]
    [InlineData(new int[] { 1 })]
    [InlineData(new int[] { 2, 3 })]
    public async Task TestAsync(int[] ids)
    {
        //Arrange
        var orders = ids
            .Select(id => new Order
            {
                InvoiceEmailAddress = "asd@example.com"
            })
            .ToArray();

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
                .SingleOrDefaultAsync(o => o.Number == order.Number, CancellationToken.None);

            Assert.NotNull(orderFromDb);
        }
    }
}
