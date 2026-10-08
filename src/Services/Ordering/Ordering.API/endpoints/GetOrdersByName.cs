


namespace Ordering.API.endpoints
{
    public record GetOrderByNameResult(IEnumerable<OrderDTO> Orders);
    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(orderName));
                var response = result.Adapt<GetOrderByNameResult>();
                return Results.Ok(result);
            })
            .WithName("GetOrderByName")
            .Produces<GetOrderByNameResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Returns the orders with the specified name");
        }
    }
}
