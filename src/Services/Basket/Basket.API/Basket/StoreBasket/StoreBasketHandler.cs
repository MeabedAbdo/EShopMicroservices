using FluentValidation;
using JasperFx.Events.Daemon;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Shopping Cart can't be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is Required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            //todo store basket in database Upsert(update if found /insert if not found )
            //todo update cache 
            await basketRepository.StoreBasket(request.Cart, cancellationToken);

            return new StoreBasketResult(request.Cart.UserName);
        }
    }
}
