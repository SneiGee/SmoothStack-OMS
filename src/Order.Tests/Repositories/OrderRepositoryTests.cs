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
    public async Task CalculateDiscountAsync_Should_Return_10Percent_For_Regular()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("DiscountDb")
            .Options;

        var context = new AppDbContext(options);
        var customerId = Guid.NewGuid();

        context.Customers.Add(new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Segment = CustomerSegment.Regular
        });
        await context.SaveChangesAsync();

        var repo = new OrderRepository(context);

        // Act
        var discount = await repo.CalculateDiscountAsync(customerId, 100, CancellationToken.None);

        // Assert
        discount.Should().Be(10.0m);
    }
}
