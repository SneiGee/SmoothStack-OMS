namespace Order.Features.CreateOrder;

public record CreateOrderResponse(
    Guid OrderId,
    decimal Total,
    decimal DiscountApplied,
    string Status);