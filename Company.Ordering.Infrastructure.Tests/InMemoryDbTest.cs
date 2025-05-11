using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public abstract class InMemoryDbTest : IDisposable
{
    private bool disposedValue;
    protected readonly OrderingDbContext _dbContext;
    public InMemoryDbTest()
    {
        var dbContextOptions = new DbContextOptionsBuilder<OrderingDbContext>()
            .UseInMemoryDatabase(nameof(CreateOrderTests))
            .Options;
        _dbContext = new OrderingDbContext(dbContextOptions);
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
