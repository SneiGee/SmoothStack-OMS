using MediatR;
using Order.Domain;
using Order.Domain.Enums;
using Order.Persistence;

namespace Order.Features.CreateOrder;

public sealed class CreateOrderHandler(
    ILogger<CreateOrderHandler> logger,
    IOrderRepository orderRepository) : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        decimal total = request.Items.Sum(i => i.Price * i.Quantity);
        decimal discount = await orderRepository.CalculateDiscountAsync(request.CustomerId, total, cancellationToken);

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            DiscountApplied = discount,
            TotalAmount = total - discount,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        await orderRepository.CreateOrderAsync(order, cancellationToken);
        logger.LogInformation("order created {OrderId}", order.Id);

        return new CreateOrderResponse(order.Id, order.TotalAmount, discount, order.Status.ToString());
    }
}
