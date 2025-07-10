using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain.Enums;

namespace Order.Features.CancelOrder;

public sealed class CancelOrderHandler(AppDbContext context)
    : IRequestHandler<CancelOrderRequest, CancelOrderResponse>
{
    public async Task<CancelOrderResponse> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException("Order not found.");

        if (order.Status is OrderStatus.Fulfilled or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order that is already {order.Status}.");

        var oldStatus = order.Status;
        order.Status = OrderStatus.Cancelled;
        order.CancelledAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return new CancelOrderResponse(
            order.Id,
            oldStatus.ToString(),
            order.Status.ToString(),
            order.CancelledAt);
    }
}
