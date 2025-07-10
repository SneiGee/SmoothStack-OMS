using MediatR;

namespace Order.Features.CancelOrder;

public record CancelOrderRequest(Guid OrderId) : IRequest<CancelOrderResponse>;
