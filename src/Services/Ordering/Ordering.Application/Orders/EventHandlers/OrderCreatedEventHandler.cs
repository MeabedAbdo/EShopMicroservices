using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> _logger) :
        INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderCreatedEventHandler received OrderCreatedEvent for OrderId: {OrderId}", notification.order.Id);
            return Task.CompletedTask;
        }
    }
}
