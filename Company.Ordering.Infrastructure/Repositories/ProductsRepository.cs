using Company.Ordering.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Company.Ordering.Infrastructure.Repositories;

public class ProductsRepository(OrderingDbContext uow)
    : Repository<Product>(uow), IProductsRepository
{
    public async Task<bool> IsInStock(int productId, int quantity)
    {
        return await uow.Products
            .AnyAsync(p => p.Id == productId && p.Stock >= quantity);
    }
}
