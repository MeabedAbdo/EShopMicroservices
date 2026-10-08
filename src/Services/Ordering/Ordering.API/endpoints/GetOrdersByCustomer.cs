using Carter;

namespace Ordering.API.endpoints
{
    public record GetOrdersByCustomerResult(IEnumerable<OrderDTO> Orders);
    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
                var response = result.Adapt<GetOrdersByCustomerResult>();
                return Results.Ok(result);
            })
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersByCustomerResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Returns the orders with the specified customer Id");
        }
    }
}
