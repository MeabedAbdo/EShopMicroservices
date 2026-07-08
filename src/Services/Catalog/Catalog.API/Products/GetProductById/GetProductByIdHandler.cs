using Catalog.API.Products.GetProducts;
using Marten;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductbyIdQuery(Guid prodcutId) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    public class GetProductByIdHandler(IDocumentSession session) : IQueryHandler<GetProductbyIdQuery, GetProductByIdResult>
    {

        public async Task<GetProductByIdResult> Handle(GetProductbyIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.prodcutId,cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(query.prodcutId);
            }
            return new GetProductByIdResult(product);
        }
    }
}
