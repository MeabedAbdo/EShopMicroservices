
using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products",
             async (ISender sender) =>
             {

                 var result = await sender.Send(new GetProductsQuery());

                 var response = result.Adapt<GetProductsResponse>();

                 return Results.Ok(response);

             })
             .WithName("GetProductList")
             .Produces<GetProductsResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get Products List")
             .WithDescription("Get Products List");
        }
    }
}
