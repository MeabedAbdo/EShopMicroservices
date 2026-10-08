using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.endpoints
{
    public record GetOrdersRequest(PaginatedRequest paginatedRequest);
    public record GetOrdersResult(PaginatedResult<OrderDTO> Orders);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] PaginatedRequest paginatedRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(paginatedRequest));
                var response = result.Adapt<GetOrdersResult>();
                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .Produces<GetOrdersResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Returns the orders with the specified pagination");
        }
    }
}
