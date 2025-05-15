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
        _product = new Product(2);
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

    [Fact]
    public async Task NotInStockByIdTestAsync()
    {
        // Act
        var isInStock = await _productsRepository.IsInStock(_product.Id + 1, _product.Stock);

        // Assert
        Assert.False(isInStock, "Product must not be in stock as absent in db");
    }

    [Fact]
    public async Task NotInStockByAmountTestAsync()
    {
        // Act
        var isInStock = await _productsRepository.IsInStock(_product.Id, _product.Stock + 1);

        // Assert
        Assert.False(isInStock, "Product must not be in stock as stock less then required");
    }
}
