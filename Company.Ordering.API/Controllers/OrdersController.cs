using Company.Ordering.Domain.OrderAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ordering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(IOrdersRepository ordersRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(Order order)
    {
        await ordersRepository.CreateOrderAsync(order);
        await ordersRepository.UnitOfWork.SaveChangesAsync();
        return Created($"/{order.Number}", order.Number);
    }

    [HttpGet("{orderNumber}")]
    public async Task<Order?> GetOrderAsync(int orderNumber)
    {
        return await ordersRepository.GetOrderAsync(orderNumber);
    }
}
