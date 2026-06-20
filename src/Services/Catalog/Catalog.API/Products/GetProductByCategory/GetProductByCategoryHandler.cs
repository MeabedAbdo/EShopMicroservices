using Marten;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string categoryName) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession _session, ILogger<GetProductByCategoryQueryHandler> _logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductByCategoryQueryHandler.Handle with {categoryName}", request.categoryName);
            var products = await _session.Query<Product>().Where(p => p.Category.Contains(request.categoryName)).ToListAsync(cancellationToken);
            return new GetProductByCategoryResult(products);
        }
    }
}
