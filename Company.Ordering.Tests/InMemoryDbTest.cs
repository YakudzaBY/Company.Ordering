using Company.Ordering.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Tests;

public abstract class InMemoryDbTest : IDisposable
{
    private bool disposedValue;
    private readonly SqliteConnection _connection;
    protected readonly OrderingDbContext _dbContext;
    public InMemoryDbTest()
    {
        // Create an in-memory SQLite connection
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var dbContextOptions = new DbContextOptionsBuilder<OrderingDbContext>()
            .UseSqlite(_connection) // Use SQLite in-memory database
            .Options;

        _dbContext = new OrderingDbContext(dbContextOptions);
        _dbContext.Database.EnsureCreated(); // Create the database schema
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                //dispose managed state (managed objects)
                _dbContext.Database.EnsureDeleted();
                _dbContext.Dispose();
                _connection.Close();
                _connection.Dispose();
            }

            //free unmanaged resources (unmanaged objects) and override finalizer
            //set large fields to null
            disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~InMemoryDbTest()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
