using Company.Ordering.API.Commands;
using Company.Ordering.API.Models;
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
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateOrderAsync(CreateOrder request)
    {
        var orderNumber = await mediator.Send(request);
        return Created($"/{orderNumber}", orderNumber);
    }

    [HttpGet("{orderNumber}")]
    [ProducesResponseType<OrderWithProducts>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<OrderWithProducts?> GetOrderWithProductsAsync(int orderNumber)
    {
        var order = await ordersRepository.GetOrderWithProductsAsync(orderNumber);
        return new OrderWithProducts
        {
            Number = order.OrderNumber,
            Products = [..order.Products
                .Select(p => new Models.OrderProduct {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    ProductAmount = p.ProductAmount
                })],
            InvoiceAddress = order.InvoiceAddress,
            InvoiceEmailAddress = order.InvoiceEmailAddress,
            InvoiceCreditCardNumber = order.InvoiceCreditCardNumber,
            CreatedAt = order.CreatedAt
        };
    }
}
