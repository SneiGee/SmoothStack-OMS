using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain;
using Order.Domain.Enums;
using Order.Features.UpdateOrderStatus;

namespace Order.Tests.Handlers;

public class UpdateOrderStatusHandlerTests
{
    [Fact]
    public async Task Should_Update_Status_From_Processing_To_Fulfilled()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("UpdateStatusDb")
            .Options;

        var context = new AppDbContext(options);
        var orderId = Guid.NewGuid();

        context.Orders.Add(new OrderEntity
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            Status = OrderStatus.Processing,
            CreatedAt = DateTime.UtcNow.AddHours(-3)
        });
        await context.SaveChangesAsync();

        var handler = new UpdateOrderStatusHandler(context);
        var request = new UpdateOrderStatusRequest(orderId, OrderStatus.Fulfilled);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.OrderId.Should().Be(orderId);
        result.OldStatus.Should().Be("Processing");
        result.NewStatus.Should().Be("Fulfilled");
        result.FulfilledAt.Should().NotBeNull();
    }
}