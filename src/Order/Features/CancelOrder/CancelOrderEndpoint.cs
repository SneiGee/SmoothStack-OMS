using Carter;
using MediatR;

namespace Order.Features.CancelOrder;

public class CancelOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders/cancel", async (
            CancelOrderRequest request,
            ISender mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        })
        .WithName("CancelOrder")
        .Produces<CancelOrderResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Cancel Order")
        .WithDescription("Cancels an existing order if it's still pending or processing.");
    }
}
