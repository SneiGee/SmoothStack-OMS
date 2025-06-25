using System.Net.Http.Json;
using FluentAssertions;
using Order.Features.CreateOrder;
using Order.Tests.TestHelpers;

namespace Order.Tests.Integration;

public class CreateOrderEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateOrderEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Create_Order_Successfully()
    {
        // Arrange
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<CreateOrderItemDto>
            {
                new(Guid.NewGuid(), 2, 10.0m)
            });

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", request);
        var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Total.Should().Be(20.0m); // no discount if customer is not seeded
        result.Status.Should().Be("Pending");
    }
}
