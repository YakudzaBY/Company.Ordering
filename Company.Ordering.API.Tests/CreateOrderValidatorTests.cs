using Company.Ordering.API.Commands;
using Company.Ordering.API.Validators;
using Company.Ordering.Domain.Aggregates.ProductAggregate;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company.Ordering.API.Tests;

public class CreateOrderValidatorTests
{
    [Fact]
    public async Task OrderFailingByEmailAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        var logger = new Mock<ILogger<CreateOrderValidator>>();
        var orderValidator = new CreateOrderValidator(productsRepository.Object, logger.Object);

        var order = new CreateOrder(
            "not-a-email",
            [],
            DateTime.UtcNow);

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.False(validationResult.IsValid, "Order should be invalid due to email");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(order.InvoiceEmailAddress));
    }

    [Fact]
    public async Task OrderFailingByOutOfStockProductAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        productsRepository
            .Setup(x => x.IsInStock(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var logger = new Mock<ILogger<CreateOrderValidator>>();
        var orderValidator = new CreateOrderValidator(productsRepository.Object, logger.Object);

        var order = new CreateOrder(
            "email@example.com",
            [new Models.OrderProduct(1, 2)],
            DateTime.UtcNow);

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.False(validationResult.IsValid, "Order should be invalid due Products out of stock");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == $"{nameof(CreateOrder.Products)}[0]");
    }

    [Fact]
    public async Task ValidOrderAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        var order = new CreateOrder(
            "valid@example.com",
            [
                new Models.OrderProduct(1,2),
                new Models.OrderProduct(2,10)
            ],
            DateTime.UtcNow
        );

        foreach (var p in order.Products)
        {

            productsRepository
                .Setup(x => x.IsInStock(p.ProductId, It.IsAny<int>()))
                .ReturnsAsync(true);
        }
        var logger = new Mock<ILogger<CreateOrderValidator>>();
        var orderValidator = new CreateOrderValidator(productsRepository.Object, logger.Object);

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.True(validationResult.IsValid, "Order should be valid with valid email and Product");
    }
}
