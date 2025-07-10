using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Data;
using Order.Domain;
using Order.Domain.Enums;
using Order.Features.CancelOrder;
using Order.Tests.TestHelpers;

namespace Order.Tests.Integration;

public class CancelOrderEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly IServiceProvider _services;

    public CancelOrderEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _services = factory.Services;
    }

    [Fact]
    public async Task Should_Cancel_Order_Successfully()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Arrange
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = 100,
            DiscountApplied = 10,
            Items = new List<OrderItem>()
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync();

        // Act ✅ Fixed Here
        var request = new CancelOrderRequest(order.Id);
        var response = await _client.PostAsJsonAsync("/api/orders/cancel", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("Cancelled");

        // Verify from DB
        var updated = await db.Orders.AsNoTracking().FirstAsync(x => x.Id == order.Id);
        updated!.Status.Should().Be(OrderStatus.Cancelled);
    }
}
