using System.Net;
using System.Net.Http.Json;
using Company.Ordering.API.Commands;
using Company.Ordering.Domain.OrderAggregate;
using Company.Ordering.Domain.ProductAggregate;
using Company.Ordering.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Ordering.API.IntegrationTests;

public class OrdersControllerIntegrationTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Product _product;

    public OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _product = new Product(2);
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>()!;
        dbContext.Products.Add(_product);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task CreateAndReadOrder()
    {
        // Arrange
        var createOrder = new CreateOrder
        {
            Products =
            [
                new Models.OrderProduct
                {
                    ProductId = _product.Id,
                    ProductAmount = 1
                }
            ],
            InvoiceEmailAddress = "test@example.com",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Orders", createOrder);

        // Assert
        var str = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var orderNumber = await response.Content.ReadFromJsonAsync<int>();

        Assert.Equal($"/{orderNumber}", response.Headers.Location?.ToString());

        var getResponse = await _client.GetAsync($"/Orders/{orderNumber}");
        Assert.True(getResponse.IsSuccessStatusCode);
    }


    [Fact]
    public async Task CreateOrder_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var createOrder = new CreateOrder
        {
            Products =
            [
                new Models.OrderProduct
                {
                    ProductId = _product.Id,
                    ProductAmount = 1
                }
            ],
            InvoiceEmailAddress = "not-an-email" // Invalid email format
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Orders", createOrder);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains(nameof(Order.InvoiceEmailAddress), problem!.Errors.Keys);
    }

    [Fact]
    public async Task CreateOrder_WithOutOfStockProduct_ReturnsBadRequest()
    {
        // Arrange
        var createOrder = new CreateOrder
        {
            Products =
            [
                new Models.OrderProduct
                {
                    ProductId = 12345,
                    ProductAmount = int.MaxValue // Invalid amount
                }
            ],
            InvoiceEmailAddress = "someone@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Orders", createOrder);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains($"{nameof(Order.Products)}[0]", problem!.Errors.Keys);
    }
}
