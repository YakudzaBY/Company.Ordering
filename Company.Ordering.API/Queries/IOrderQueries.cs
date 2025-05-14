using Company.Ordering.API.Models;

namespace Company.Ordering.API.Queries
{
    public interface IOrderQueries
    {
        Task<OrderWithProducts?> GetOrderWithProductsAsync(int orderNumber, CancellationToken cancellationToken = default);
    }
}
