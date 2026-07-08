
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
           string Name,
           List<string> Category,
           string Description,
           string ImageFile,
           decimal Price) : ICommand<CreateProductResult>; 
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file path is required.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Product price is required").GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }

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
            //save product object to DB 
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // return result
            return new CreateProductResult(Guid.NewGuid()); // Simulate product creation and return a new Guid as Id
        }
    }
}
