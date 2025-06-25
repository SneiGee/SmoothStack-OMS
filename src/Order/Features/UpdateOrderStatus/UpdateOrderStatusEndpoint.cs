using Carter;
using MediatR;

namespace Order.Features.UpdateOrderStatus;

public class UpdateOrderStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/orders/status", async (
            UpdateOrderStatusRequest request,
            ISender mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateOrderStatusEndpoint))
        .Produces<UpdateOrderStatusResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Order Status")
        .WithDescription("Updates the status of an order and handles fulfillment time if applicable.");
    }
}