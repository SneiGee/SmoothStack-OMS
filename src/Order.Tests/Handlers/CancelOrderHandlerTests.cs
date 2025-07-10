using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain;
using Order.Domain.Enums;
using Order.Features.CancelOrder;

namespace Order.Tests.Handlers;

public class CancelOrderHandlerTests
{
    [Fact]
    public async Task Should_Cancel_Order_And_Return_Response()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var order = new OrderEntity
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var handler = new CancelOrderHandler(context);
        var request = new CancelOrderRequest(orderId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OrderId.Should().Be(orderId);
        result.OldStatus.Should().Be("Pending");
        result.NewStatus.Should().Be("Cancelled");
        result.CancelledAt.Should().NotBeNull();

        var updated = await context.Orders.AsNoTracking().FirstAsync(x => x.Id == orderId);
        updated.Status.Should().Be(OrderStatus.Cancelled);
    }
}