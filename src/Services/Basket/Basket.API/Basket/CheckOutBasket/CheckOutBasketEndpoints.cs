
using Mapster;

namespace Basket.API.Basket.CheckOutBasket
{
    public record CheckOutBasketRequest(CheckOutBasketDTO CheckOutBasketDto);
    public record CheckOutBasketResponse(bool Success);
    public class CheckOutBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckOutBasketRequest request, ISender _sender) =>
            {
                var command = request.Adapt<CheckOutBasketCommand>();
                var result = await _sender.Send(command);
                var response = result.Adapt<CheckOutBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("CheckoutBasket")
        .Produces<CheckOutBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");
        }
    }
}
