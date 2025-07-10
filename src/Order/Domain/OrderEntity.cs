using Order.Domain.Enums;

namespace Order.Domain;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal DiscountApplied { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}
