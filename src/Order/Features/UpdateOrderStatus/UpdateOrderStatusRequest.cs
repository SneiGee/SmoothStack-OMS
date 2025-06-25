using MediatR;
using Order.Domain.Enums;

namespace Order.Features.UpdateOrderStatus;

public record UpdateOrderStatusRequest(
    Guid OrderId,
    OrderStatus NewStatus
) : IRequest<UpdateOrderStatusResponse>;
