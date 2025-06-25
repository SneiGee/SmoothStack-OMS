using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Domain;
using Order.Features.CreateOrder;
using Order.Persistence;

namespace Order.Tests.Handlers;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _mockRepo;
    private readonly Mock<ILogger<CreateOrderHandler>> _mockLogger;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _mockRepo = new Mock<IOrderRepository>();
        _mockLogger = new Mock<ILogger<CreateOrderHandler>>();
        _handler = new CreateOrderHandler(_mockLogger.Object, _mockRepo.Object);
    }

    [Fact]
    public async Task Should_Create_Order_And_Return_Response()
    {
        // arrange
        var customerId = Guid.NewGuid();
        var request = new CreateOrderRequest(
            customerId,
            new List<CreateOrderItemDto>
            {
                new(Guid.NewGuid(), 2, 10.0m)
            });

        _mockRepo.Setup(x => x.CalculateDiscountAsync(customerId, 20.0m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2.0m);

        // act
        var response = await _handler.Handle(request, CancellationToken.None);

        // assert
        response.Should().NotBeNull();
        response.Total.Should().Be(18.0m);
        response.DiscountApplied.Should().Be(2.0m);
        response.Status.Should().Be("Pending");

        _mockRepo.Verify(x => x.CreateOrderAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
