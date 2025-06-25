using MediatR;

namespace Order.Features.CreateOrder;

public record CreateOrderRequest(
    Guid CustomerId,
    List<CreateOrderItemDto> Items) : IRequest<CreateOrderResponse>;

public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price);
