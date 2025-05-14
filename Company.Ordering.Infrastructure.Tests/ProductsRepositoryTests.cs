using Company.Ordering.Domain.ProductAggregate;
using Company.Ordering.Infrastructure.Repositories;
using Company.Ordering.Tests;

namespace Company.Ordering.Infrastructure.Tests;

public class ProductsRepositoryTests : InMemoryDbTest
{
    private readonly Product _product;
    private readonly ProductsRepository _productsRepository;

    public ProductsRepositoryTests() : base()
    {
        _product = new Product { Id = 2, Stock = 2 };
        _dbContext.Products.Add(_product);
        _dbContext.SaveChanges();

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
    [InlineData(2, 3, "Product must not be in stock as stock is stock=2")]
    public async Task NotInStockTestAsync(int productId, int stock, string message)
    {
        // Act
        var isInStock = await _productsRepository.IsInStock(productId, stock);

        // Assert
        Assert.False(isInStock, message);
    }
}
