using Company.Ordering.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Tests;

public abstract class InMemoryDbTest : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly Func<Task<OrderingDbContext>> GetDbContextAsync;

    public InMemoryDbTest()
    {
        // Create an in-memory SQLite connection
        _connection = new SqliteConnection("DataSource=:memory:");
        var connectionTask = _connection.OpenAsync();

        GetDbContextAsync = async () =>
        {
            await connectionTask;

            var dbContextOptions = new DbContextOptionsBuilder<OrderingDbContext>()
                .UseSqlite(_connection) // Use SQLite in-memory database
                .Options;

            var dbContext = new OrderingDbContext(dbContextOptions);

            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        };
    }

    public async ValueTask DisposeAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.DisposeAsync();
        await _connection.CloseAsync();
        await _connection.DisposeAsync();
    }
}
