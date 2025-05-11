using Company.Ordering.Domain.ProductAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Tests;

public class ProductsRepositoryTests : IDisposable
{
    private readonly OrderingDbContext _dbContext;
    private readonly Product _product;
    private readonly ProductsRepository _productsRepository;

    public ProductsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<OrderingDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ProductsRepositoryTests))
            .Options;

        _dbContext = new OrderingDbContext(options);

        _product = new Product { Id = 12345, Stock = 2 };
        using var dbContext = new OrderingDbContext(options);
        dbContext.Products.Add(_product);
        dbContext.SaveChanges();

        _productsRepository = new ProductsRepository(_dbContext);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task InStockTestAsync(int amount)
    {
        // Act
        var isInStock = await _productsRepository.IsInStock(_product.Id, amount);

        // Assert
        Assert.True(isInStock, "Product must be in stock");
    }

    [Theory]
    [InlineData(1, 1, "Product must not be in stock as absent in db")]
    [InlineData(12345, 3, "Product must not be in stock as stock is stock=2")]
    public async Task NotInStockTestAsync(int productId, int stock, string message)
    {
        // Act
        var isInStock = await _productsRepository.IsInStock(productId, stock);

        // Assert
        Assert.False(isInStock, message);
    }

    public void Dispose()
    {
        // Cleanup: Clear the database
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
