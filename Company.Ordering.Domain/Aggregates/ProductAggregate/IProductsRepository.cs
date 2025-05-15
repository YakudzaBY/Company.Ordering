namespace Company.Ordering.Domain.Aggregates.ProductAggregate;

public interface IProductsRepository : IRepository<Product>
{
    Task<bool> IsInStock(int productId, int quantity);
}
