

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDTO Order) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool IsSuccess);

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order.Id)
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("Order Id is required.");

            RuleFor(x => x.Order.OrderName)
                .NotEmpty()
                .WithMessage("OrderName is required.");

            RuleFor(x => x.Order.CustomerId)
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is required.");
        }
    }
}
