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
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

        if (customer is null)
            return 0;

        return customer.Segment switch
        {
            CustomerSegment.New => total * 0.05m,
            CustomerSegment.Regular => total * 0.10m,
            CustomerSegment.VIP => total * 0.20m,
            _ => 0
        };
    }
}
