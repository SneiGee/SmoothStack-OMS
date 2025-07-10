using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain;
using Order.Domain.Enums;

namespace Order.Persistence;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task CreateOrderAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<decimal> CalculateDiscountAsync(Guid customerId, decimal total, CancellationToken cancellationToken)
    {
        var customer = await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

        if (customer is null)
            return 0;

        decimal baseDiscount = customer.Segment switch
        {
            CustomerSegment.New => total * 0.05m,
            CustomerSegment.Regular => total * 0.10m,
            CustomerSegment.VIP => total * 0.20m,
            _ => 0
        };

        // Loyalty bonus: orders in last 90 days
        var ninetyDaysAgo = DateTime.UtcNow.AddDays(-90);

        int recentOrdersCount = await context.Orders
            .AsNoTracking()
            .CountAsync(o =>
                o.CustomerId == customerId &&
                o.Status == OrderStatus.Fulfilled &&
                o.FulfilledAt != null &&
                o.FulfilledAt >= ninetyDaysAgo,
                cancellationToken);

        // Loyalty discount
        decimal loyaltyBonus = recentOrdersCount >= 3 ? total * 0.05m : 0;

        // Total disCount
        return baseDiscount + loyaltyBonus;
    }
}
