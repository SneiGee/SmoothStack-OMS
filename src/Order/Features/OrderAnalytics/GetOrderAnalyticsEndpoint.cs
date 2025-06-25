using Carter;
using MediatR;

namespace Order.Features.OrderAnalytics;

public class GetOrderAnalyticsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/analytics", async (ISender mediator) =>
        {
            var result = await mediator.Send(new GetOrderAnalyticsRequest());
            return Results.Ok(result);
        })
        .WithName(nameof(GetOrderAnalyticsEndpoint))
        .Produces<GetOrderAnalyticsResponse>()
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Get Order Analytics")
        .WithDescription("Returns stats such as average order value and average fulfillment time.");
    }
}