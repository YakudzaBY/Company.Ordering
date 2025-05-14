using Company.Ordering.API.Commands;
using Company.Ordering.API.Model;
using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
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
    public async Task<ActionResult<int>> CreateOrderAsync(CreateOrder request)
    {
        var orderNumber = await mediator.Send(request);
        return Created($"/{orderNumber}", orderNumber);
    }

    [HttpGet("{orderNumber}")]
    public async Task<ActionResult<Order?>> GetOrderWithProductsAsync(string orderNumber)
    {
        Order? order = null;
        if (int.TryParse(orderNumber, out var intOrderNumber))
        {
            order = await ordersRepository.GetOrderWithProductsAsync(intOrderNumber);
        }
        else if (Guid.TryParse(orderNumber, out var guidOrderNumber))
        {
            order = await ordersRepository.GetOrderWithProductsAsync(guidOrderNumber);
        }
        else
        {
            return BadRequest("Invalid order number format.");
        }

        var orderResponse = order == null
            ? null
            : new OrderWithProducts
            {
                Number = order.Number,
                InvoiceEmailAddress = order.InvoiceEmailAddress,
                CreatedAt = order.CreatedAt,
                InvoiceAddress = order.InvoiceAddress,
                InvoiceCreditCardNumber = order.InvoiceCreditCardNumber,
                Products = [..order
                    .Products
                    .Select(p => p.Clone())]
                    
            };
        return orderResponse;
    }
}
