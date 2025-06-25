namespace Order.Features.OrderAnalytics;

public record GetOrderAnalyticsResponse(
    int TotalOrders,
    int FulfilledOrders,
    decimal AverageOrderValue,
    double AverageFulfillmentTimeInHours);