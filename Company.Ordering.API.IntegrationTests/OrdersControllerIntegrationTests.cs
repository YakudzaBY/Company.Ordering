using System.Net;
using System.Net.Http.Json;
using Company.Ordering.API.Commands;
using Company.Ordering.Domain.Aggregates.OrderAggregate;
using Company.Ordering.Domain.Aggregates.ProductAggregate;
using Company.Ordering.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Ordering.API.IntegrationTests;

public class OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly Func<Task<Product>> GetProductAsync = async () =>
        {
            var product = new Product(2);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>()!;
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        };

    [Fact]
    public async Task CreateAndReadOrder()
    {
        // Arrange
        var product = await GetProductAsync();
        var createOrder = new CreateOrder(
            "test@example.com",
            [new Models.OrderProduct(product.Id, 1)],
            DateTime.UtcNow
            );
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
        var product = await GetProductAsync();
        var createOrder = new CreateOrder(
            "not-an-email",
            [new Models.OrderProduct(product.Id, 1)],
            DateTime.UtcNow);

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
        var product = await GetProductAsync();
        var createOrder = new CreateOrder(
            "test@example.com",
            [new Models.OrderProduct(product.Id, int.MaxValue)],
            DateTime.UtcNow);

        // Act
        var response = await _client.PostAsJsonAsync("/Orders", createOrder);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains($"{nameof(Order.Products)}[0]", problem!.Errors.Keys);
    }
}
