using FluentValidation;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDTO Order) : ICommand<CreateOrderResult>;

    public record CreateOrderResult(Guid Id);

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order.OrderName)
                .NotEmpty()
                .WithMessage("OrderName is required.");

            RuleFor(x => x.Order.CustomerId)
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.Order.OrderItems)
                .NotNull()
                .WithMessage("OrderItems cannot be null.")
                .NotEmpty()
                .WithMessage("OrderItems cannot be empty.");
        }
    }
}
