using Order.Domain;

namespace Order.Persistence;

public interface IOrderRepository
{
    Task CreateOrderAsync(OrderEntity order, CancellationToken cancellationToken);
    Task<decimal> CalculateDiscountAsync(Guid customerId, decimal total, CancellationToken cancellationToken);
}
