using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using Moq;

namespace Company.Ordering.Domain.Tests;

public class OrdersValidatorTests
{
    [Fact]
    public async Task OrderFailingByEmailAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        var orderValidator = new OrderValidator(productsRepository.Object);

        var order = new Order
        {
            InvoiceEmailAddress = "notcorrectemail",
        };

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

        var orderValidator = new OrderValidator(productsRepository.Object);

        var order = new Order
        {
            Products = [
                new OrderProduct
                {
                    ProductId = 1,
                    Amount = 2,
                },
            ],
        };

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.False(validationResult.IsValid, "Order should be invalid due Products out of stock");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == $"{nameof(Order.Products)}[0]");
    }

    [Fact]
    public async Task ValidOrderAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        var order = new Order
        {
            InvoiceEmailAddress = "valid@example.com",
            Products = [
                new OrderProduct
                {
                    ProductId = 1,
                    Amount = 2,
                },
                new OrderProduct
                {
                    ProductId = 2,
                    Amount = 10,
                },
            ],
        };

        foreach(var p in order.Products)
        {

            productsRepository
                .Setup(x => x.IsInStock(p.ProductId, It.IsAny<int>()))
                .ReturnsAsync(true);
        }

        var orderValidator = new OrderValidator(productsRepository.Object);

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.True(validationResult.IsValid, "Order should be valid with valid email and Product");
    }
}