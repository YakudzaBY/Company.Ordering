using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public class CreateOrderTests: IDisposable
{

    private readonly OrderingDbContext _dbContext;
    public CreateOrderTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<OrderingDbContext>()
            .UseInMemoryDatabase(nameof(CreateOrderTests))
            .Options;
        _dbContext = new OrderingDbContext(dbContextOptions);
    }

    [Fact]
    public async Task CreateOrderTestAsync()
    {
        //Arrange
        var order = new Order
        {
            Number = 1,
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

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
