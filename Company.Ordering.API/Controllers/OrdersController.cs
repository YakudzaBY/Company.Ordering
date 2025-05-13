using Company.Ordering.API.Commands;
using Company.Ordering.Domain.OrderAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ordering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(
    IOrdersRepository ordersRepository,
    IMediator mediator)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(CreateOrder request)
    {
        var orderNumber = await mediator.Send(request);
        return Created($"/{orderNumber}", orderNumber);
    }

    [HttpGet("{orderNumber}")]
    public async Task<Order?> GetOrderWithProductsAsync(int orderNumber)
    {
        return await ordersRepository.GetOrderWithProductsAsync(orderNumber);
    }
}
