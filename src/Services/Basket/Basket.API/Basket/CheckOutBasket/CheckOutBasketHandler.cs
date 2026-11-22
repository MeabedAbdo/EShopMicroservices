


using Mapster;

namespace Basket.API.Basket.CheckOutBasket
{
    public record CheckOutBasketCommand(CheckOutBasketDTO CheckOutBasketDTO) : ICommand<CheckOutBasketResult>;
    public record CheckOutBasketResult(bool Success);

    public class CheckOutBasketCommandValidator : AbstractValidator<CheckOutBasketCommand>
    {
        public CheckOutBasketCommandValidator()
        {
            RuleFor(x => x.CheckOutBasketDTO).NotNull().WithMessage("CheckOutBasketDTO can't be null");
            RuleFor(x => x.CheckOutBasketDTO.UserName).NotEmpty().WithMessage("UserName is Required");

        }
    }
    public class CheckOutBasketHandler(IBasketRepository _basketRepository, IPublishEndpoint _publishEndpoint) : ICommandHandler<CheckOutBasketCommand, CheckOutBasketResult>
    {
        public async Task<CheckOutBasketResult> Handle(CheckOutBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasket(command.CheckOutBasketDTO.UserName);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.CheckOutBasketDTO.UserName);
            }
            var eventMessage=command.CheckOutBasketDTO.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage, cancellationToken);
            await _basketRepository.DeleteBasket(command.CheckOutBasketDTO.UserName,cancellationToken);

            // Implementation for handling the check-out basket command
            return new CheckOutBasketResult(true);
        }
    }
}
