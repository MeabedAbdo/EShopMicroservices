using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.API.Products.GetProducts
{

    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {

            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize?? 10, cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
