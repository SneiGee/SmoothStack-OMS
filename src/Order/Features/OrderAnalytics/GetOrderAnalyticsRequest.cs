using MediatR;

namespace Order.Features.OrderAnalytics;

public record GetOrderAnalyticsRequest() : IRequest<GetOrderAnalyticsResponse>;
