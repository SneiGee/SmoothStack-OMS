using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Data;

namespace Order.Features.OrderAnalytics;

public sealed class GetOrderAnalyticsHandler(AppDbContext context)
    : IRequestHandler<GetOrderAnalyticsRequest, GetOrderAnalyticsResponse>
{
    public async Task<GetOrderAnalyticsResponse> Handle(GetOrderAnalyticsRequest request, CancellationToken cancellationToken)
    {
        var totalOrders = await context.Orders.CountAsync(cancellationToken);
        var fulfilledOrders = await context.Orders
            .Where(o => o.Status == Domain.Enums.OrderStatus.Fulfilled)
            .ToListAsync(cancellationToken);

        decimal avgOrderValue = totalOrders > 0
            ? await context.Orders.AverageAsync(o => o.TotalAmount, cancellationToken)
            : 0;

        double avgFulfillmentHours = fulfilledOrders.Count > 0
            ? fulfilledOrders.Average(o => (o.FulfilledAt!.Value - o.CreatedAt).TotalHours)
            : 0;

        return new GetOrderAnalyticsResponse(
            TotalOrders: totalOrders,
            FulfilledOrders: fulfilledOrders.Count,
            AverageOrderValue: Math.Round(avgOrderValue, 2),
            AverageFulfillmentTimeInHours: Math.Round(avgFulfillmentHours, 2));
    }
}