using Company.Ordering.API.Commands;
using Company.Ordering.API.Models;
using Company.Ordering.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ordering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(
    IOrderQueries orderQueries,
    IMediator mediator,
    ILogger<OrdersController> logger)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateOrderAsync(CreateOrder request)
    {
        logger.LogInformation("----- Sending command: {CommandName}", request.GetType());

        var orderNumber = await mediator.Send(request);
        return Created($"/{orderNumber}", orderNumber);
    }

    [HttpGet("{orderNumber}")]
    [ProducesResponseType<OrderWithProducts>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<OrderWithProducts?> GetOrderWithProductsAsync(int orderNumber)
    {
        return await orderQueries.GetOrderWithProductsAsync(orderNumber);
    }
}
