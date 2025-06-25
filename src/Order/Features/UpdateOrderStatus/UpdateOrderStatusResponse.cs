namespace Order.Features.UpdateOrderStatus;

public record UpdateOrderStatusResponse(
    Guid OrderId,
    string OldStatus,
    string NewStatus,
    DateTime? FulfilledAt);
