using Company.Ordering.API.CommandHandlers;
using Company.Ordering.API.Commands;
using Company.Ordering.Domain;
using Company.Ordering.Domain.Aggregates.OrderAggregate;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company.Ordering.API.Tests;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_AssignsOrderNumber_AfterSaveChangesAsync()
    {
        // Arrange
        var createOrder = new CreateOrder("test@example.com", [
            new Models.OrderProduct(1, 2)],
            DateTime.UtcNow);

        const int newOrderNumber = 42;

        var mockRepo = new Mock<IOrdersRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        Order? capturedOrder = null;

        mockRepo.Setup(r => r.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        mockRepo.SetupGet(r => r.UnitOfWork).Returns(mockUnitOfWork.Object);

        mockUnitOfWork.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                // Simulate DB assigning OrderNumber after save
                if (capturedOrder != null)
                {
                    capturedOrder.GetType().GetProperty(nameof(Order.Id))!.SetValue(capturedOrder, newOrderNumber);
                }
            });
        var logger = new Mock<ILogger<CreateOrderHandler>>();
        var handler = new CreateOrderHandler(mockRepo.Object, logger.Object);

        // Act
        var result = await handler.Handle(createOrder, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(newOrderNumber, result);
    }
}