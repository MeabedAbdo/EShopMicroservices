using Discount.Grpc.Protos;
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
    public class StoreBasketCommandHandler(IBasketRepository basketRepository,DiscountProtoService.DiscountProtoServiceClient discountSrvcClient) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            //todo store basket in database Upsert(update if found /insert if not found )
            //todo update cache 
            var cart = await DeductDiscount(request.Cart, cancellationToken);
            await basketRepository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }

        private async Task<ShoppingCart> DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountSrvcClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
            return cart;
        }
    }
}
