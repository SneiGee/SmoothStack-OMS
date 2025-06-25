using Order.Domain.Enums;

namespace Order.Domain;

public class Customer
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public CustomerSegment Segment { get; set; }
}
