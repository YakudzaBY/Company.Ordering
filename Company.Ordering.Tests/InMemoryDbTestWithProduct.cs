using Company.Ordering.Domain.Aggregates.ProductAggregate;

namespace Company.Ordering.Tests;

public abstract class InMemoryDbTestWithProduct : InMemoryDbTest
{
    protected readonly Func<Task<Product>> EnsureProductAsync;

    public InMemoryDbTestWithProduct() : base()
    {
        EnsureProductAsync = async () =>
        {
            var product = new Product(2);
            var dbContext = await GetDbContextAsync();
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        };
    }
}
