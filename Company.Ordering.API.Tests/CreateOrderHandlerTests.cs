using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Company.Ordering.API.CommandHandlers;
using Company.Ordering.API.Commands;
using Company.Ordering.Domain;
using Company.Ordering.Domain.OrderAggregate;
using Moq;
using Xunit;

namespace Company.Ordering.API.Tests;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_AssignsOrderNumber_AfterSaveChangesAsync()
    {
        // Arrange
        var createOrder = new CreateOrder
        {
            Products = [
            new Company.Ordering.API.Models.OrderProduct
            {
                ProductId = 1,
                ProductName = "Test Product",
                ProductAmount = 2,
                ProductPrice = 10.0m
            }],
            InvoiceAddress = "123 Test St",
            InvoiceEmailAddress = "test@example.com",
            InvoiceCreditCardNumber = "4111111111111111"
        };

        const int newOrderNumber = 42;

        var mockRepo = new Mock<IOrdersRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        Order? capturedOrder = null;

        mockRepo.Setup(r => r.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        mockRepo.SetupGet(r => r.UnitOfWork).Returns(mockUnitOfWork.Object);

        mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                // Simulate DB assigning OrderNumber after save
                if (capturedOrder != null)
                    capturedOrder.OrderNumber = newOrderNumber;
            })
            .ReturnsAsync(1);

        var handler = new CreateOrderHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(createOrder, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(newOrderNumber, result);
    }
}