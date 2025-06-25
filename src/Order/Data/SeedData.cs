using Order.Domain;
using Order.Domain.Enums;

namespace Order.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Customers.Any())
            return; // Already seeded

        var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), Name = "Alice", Segment = CustomerSegment.New },
            new Customer { Id = Guid.NewGuid(), Name = "Bob", Segment = CustomerSegment.Regular },
            new Customer { Id = Guid.NewGuid(), Name = "Charlie", Segment = CustomerSegment.VIP }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}