namespace Company.Ordering.Domain.ProductAggregate;

public interface IProductsRepository : IRepository<Product>
{
    Task<bool> IsInStock(int productId, int quantity);
}
