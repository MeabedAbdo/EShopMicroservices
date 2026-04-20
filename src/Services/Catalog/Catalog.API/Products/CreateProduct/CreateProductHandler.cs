using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
           string Name,
           List<string> Category,
           string Description,
           string ImageFile,
           decimal Price) : ICommand<CreateProductResult>; // Fix for CS0311: Ensure CreateProductCommand implements IRequest<CreateProductResult>
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };
            //ToDo
            // Here you would typically save the product to a database or perform some other action.
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(Guid.NewGuid()); // Simulate product creation and return a new Guid as Id
        }
    }
}
