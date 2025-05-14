using Company.Ordering.API.Commands;
using Company.Ordering.API.Validators;
using Company.Ordering.Domain.ProductAggregate;
using Moq;

namespace Company.Ordering.API.Tests;

public class CreateOrderValidatorTests
{
    [Fact]
    public async Task OrderFailingByEmailAsync()
    {
        //Arrange
        var productsRepository = new Mock<IProductsRepository>();

        var orderValidator = new CreateOrderValidator(productsRepository.Object);

        var order = new CreateOrder
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

        var orderValidator = new CreateOrderValidator(productsRepository.Object);

        var order = new CreateOrder
        {
            Products = [
                new Models.OrderProduct
                {
                    ProductId = 1,
                    ProductAmount = 2,
                },
            ],
        };

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

        var order = new CreateOrder
        {
            InvoiceEmailAddress = "valid@example.com",
            Products = [
                new Models.OrderProduct
                {
                    ProductId = 1,
                    ProductAmount = 2,
                },
                new Models.OrderProduct
                {
                    ProductId = 2,
                    ProductAmount = 10,
                },
            ],
        };

        foreach (var p in order.Products)
        {

            productsRepository
                .Setup(x => x.IsInStock(p.ProductId, It.IsAny<int>()))
                .ReturnsAsync(true);
        }

        var orderValidator = new CreateOrderValidator(productsRepository.Object);

        //Act
        var validationResult = await orderValidator.ValidateAsync(order);

        //Assert
        Assert.True(validationResult.IsValid, "Order should be valid with valid email and Product");
    }
}
