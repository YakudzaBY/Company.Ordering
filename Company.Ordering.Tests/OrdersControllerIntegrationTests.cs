using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Company.Ordering.Domain.OrderAggregate;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Company.Ordering.Tests;

public class OrdersControllerIntegrationTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreated()
    {
        // Arrange
        var createOrder = new
        {
            Products = new[]
            {
                new { ProductId = 12345, Name = "Test Product", Amount = 1, Price = 9.99m }
            },
            InvoiceAddress = "123 Main St",
            InvoiceEmailAddress = "test@example.com",
            InvoiceCreditCardNumber = "4111111111111111"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Orders", createOrder);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var orderNumber = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(orderNumber);
    }

    [Fact]
    public async Task GetOrder_ReturnsOrder()
    {
        // Arrange: First, create an order
        var createOrder = new
        {
            Products = new[]
            {
                new { ProductId = 12345, Name = "Another Product", Amount = 2, Price = 19.99m }
            },
            InvoiceAddress = "456 Main St",
            InvoiceEmailAddress = "another@example.com",
            InvoiceCreditCardNumber = "4111111111111111"
        };
        var createResponse = await _client.PostAsJsonAsync("/Orders", createOrder);
        var orderNumber = await createResponse.Content.ReadAsStringAsync();

        // Act
        var response = await _client.GetAsync($"/Orders/{orderNumber}");

        // Assert
        Assert.True(response.IsSuccessStatusCode, "Response should be successful");
        var order = await response.Content.ReadFromJsonAsync<Order>();
        Assert.NotNull(order);
        Assert.Equal(orderNumber, order!.Number.ToString());
    }
}
