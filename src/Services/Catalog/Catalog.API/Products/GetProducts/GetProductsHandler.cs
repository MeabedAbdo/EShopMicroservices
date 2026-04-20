using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.API.Products.GetProducts
{

    public record GetProductsQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    public class GetProductsHandler(IDocumentSession session, ILogger<GetProductsHandler> _logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductsQueryHandler.Handle with {query}", query);
            
            var products = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
