using Company.Ordering.API.Commands;
using Company.Ordering.API.Controllers;
using Company.Ordering.API.Models;
using Company.Ordering.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company.Ordering.API.Tests;

public class OrdersControllerTests
{
    const int orderId = 123;

    [Fact]
    public async Task CreateOrderAsync_ReturnsCreatedResult_WithOrderNumber()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var queriesMock = new Mock<IOrderQueries>();
        var createOrder = new CreateOrder("", [], default);

        mediatorMock
            .Setup(m => m.Send(createOrder, It.IsAny<CancellationToken>()))
            .ReturnsAsync(orderId);
        var logger = new Mock<ILogger<OrdersController>>();
        var controller = new OrdersController(queriesMock.Object, mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.CreateOrderAsync(createOrder);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(orderId, createdResult.Value);
        Assert.Equal($"/{orderId}", createdResult.Location);
    }

    [Fact]
    public async Task GetOrderWithProductsAsync_ReturnsOrder_WhenFound()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var queriesMock = new Mock<IOrderQueries>();
        var order = new OrderWithProducts { OrderNumber = orderId };
        queriesMock
            .Setup(q => q.GetOrderWithProductsAsync(order.OrderNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var logger = new Mock<ILogger<OrdersController>>();
        var controller = new OrdersController(queriesMock.Object, mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.GetOrderWithProductsAsync(order.OrderNumber);

        // Assert
        Assert.Equal(order, result);
    }

    [Fact]
    public async Task GetOrderWithProductsAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var queriesMock = new Mock<IOrderQueries>();
        queriesMock
            .Setup(q => q.GetOrderWithProductsAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderWithProducts?)null);

        var logger = new Mock<ILogger<OrdersController>>();
        var controller = new OrdersController(queriesMock.Object, mediatorMock.Object, logger.Object);

        // Act
        var result = await controller.GetOrderWithProductsAsync(orderId);

        // Assert
        Assert.Null(result);
    }
}
