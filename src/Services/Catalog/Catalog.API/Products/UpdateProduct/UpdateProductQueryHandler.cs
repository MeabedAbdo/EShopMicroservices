using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool Success);

    public class UpdateProductQueryHandler(IDocumentSession _session, ILogger<UpdateProductQueryHandler> _logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateProductQueryHandler.Handle with Product ID: {ProductId}", command.Id);

            var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            product.Name = command.Name;
            product.Category = command.Category;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            _session.Update(product);
            await _session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);

        }
    }
}