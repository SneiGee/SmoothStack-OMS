using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain;
using Order.Domain.Enums;
using Order.Persistence;

namespace Order.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task CalculateDiscountAsync_Should_Apply_Loyalty_Bonus_When_Eligible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var customerId = Guid.NewGuid();
        var totalAmount = 100m;

        context.Customers.Add(new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Segment = CustomerSegment.VIP
        });
        
        // Seed 3 fulfilled orders in last 90 days
        var now = DateTime.UtcNow;
        for (int i = 0; i < 3; i++)
        {
            context.Orders.Add(new OrderEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Status = OrderStatus.Fulfilled,
                CreatedAt = now.AddDays(-80 - i), // within 90 days
                FulfilledAt = now.AddDays(-80 - i),
                TotalAmount = 50,
                DiscountApplied = 0
            });
        }

        await context.SaveChangesAsync();

        var repo = new OrderRepository(context);

        // Act
        var discount = await repo.CalculateDiscountAsync(customerId, totalAmount, CancellationToken.None);

        // Assert
        discount.Should().Be(25); // 20% VIP + 5% loyalty = 25% of 100
    }
}
