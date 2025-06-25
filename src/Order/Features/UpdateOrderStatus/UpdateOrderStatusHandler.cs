using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Domain.Enums;
using Order.Shared.Exceptions;

namespace Order.Features.UpdateOrderStatus;

public sealed class UpdateOrderStatusHandler(AppDbContext db)
    : IRequestHandler<UpdateOrderStatusRequest, UpdateOrderStatusResponse>
{
    public async Task<UpdateOrderStatusResponse> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var order = await db.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new Exception("Order not found.");

        var oldStatus = order.Status;

        if (!IsValidTransition(oldStatus, request.NewStatus))
            throw new BadRequestException($"Invalid status transition from {oldStatus} to {request.NewStatus}");

        order.Status = request.NewStatus;

        if (request.NewStatus == OrderStatus.Fulfilled)
        {
            order.FulfilledAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(cancellationToken);

        return new UpdateOrderStatusResponse(
            order.Id,
            oldStatus.ToString(),
            order.Status.ToString(),
            order.FulfilledAt
        );
    }

    private bool IsValidTransition(OrderStatus current, OrderStatus next)
    {
        return current switch
        {
            OrderStatus.Pending => next == OrderStatus.Processing,
            OrderStatus.Processing => next is OrderStatus.Fulfilled or OrderStatus.Cancelled,
            _ => false
        };
    }
}