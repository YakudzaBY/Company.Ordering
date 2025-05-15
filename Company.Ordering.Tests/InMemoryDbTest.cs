using Company.Ordering.Infrastructure;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Company.Ordering.Tests;

public abstract class InMemoryDbTest : IAsyncDisposable, IDisposable
{
    private bool _disposed;
    private readonly SqliteConnection _connection;
    protected readonly Func<Task<OrderingDbContext>> GetDbContextAsync;

    public InMemoryDbTest()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        var connectionTask = _connection.OpenAsync();
        var mediatr = new Mock<IMediator>(); //TODO check

        GetDbContextAsync = async () =>
        {
            await connectionTask;

            var dbContextOptions = new DbContextOptionsBuilder<OrderingDbContext>()
                .UseSqlite(_connection)
                .Options;

            var dbContext = new OrderingDbContext(dbContextOptions, mediatr.Object);

            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        };
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.DisposeAsync();
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Synchronous cleanup of managed resources
                var dbContext = GetDbContextAsync().GetAwaiter().GetResult();
                dbContext.Database.EnsureDeleted();
                dbContext.Dispose();
                _connection.Close();
                _connection.Dispose();
            }
            _disposed = true;
        }
    }
}