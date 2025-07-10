namespace Order.Features.CancelOrder;

public record CancelOrderResponse(
    Guid OrderId,
    string OldStatus,
    string NewStatus,
    DateTime? CancelledAt);
