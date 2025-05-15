using Company.Ordering.Infrastructure.Repositories;
using Company.Ordering.Tests;

namespace Company.Ordering.Infrastructure.Tests;

public class ProductsRepositoryTests : InMemoryDbTestWithProduct
{
    private readonly Func<Task<ProductsRepository>> GetProductsRepositoryAsync;

    public ProductsRepositoryTests() : base()
    {
        GetProductsRepositoryAsync = async () =>
        {
            var dbContext = await GetDbContextAsync();
            return new ProductsRepository(dbContext);
        };
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task InStockTestAsync(int amount)
    {
        // Arrange
        var productsRepository = await GetProductsRepositoryAsync();
        var product = await EnsureProductAsync();

        // Act
        var isInStock = await productsRepository.IsInStock(product.Id, amount);

        // Assert
        Assert.True(isInStock, "Product must be in stock");
    }

    [Fact]
    public async Task NotInStockByIdTestAsync()
    {
        // Arrange
        var productsRepository = await GetProductsRepositoryAsync();
        var product = await EnsureProductAsync();

        // Act
        var isInStock = await productsRepository.IsInStock(product.Id + 1, product.Stock);

        // Assert
        Assert.False(isInStock, "Product must not be in stock as absent in db");
    }

    [Fact]
    public async Task NotInStockByAmountTestAsync()
    {
        // Arrange
        var productsRepository = await GetProductsRepositoryAsync();
        var product = await EnsureProductAsync();

        // Act
        var isInStock = await productsRepository.IsInStock(product.Id, product.Stock + 1);

        // Assert
        Assert.False(isInStock, "Product must not be in stock as stock less then required");
    }
}
