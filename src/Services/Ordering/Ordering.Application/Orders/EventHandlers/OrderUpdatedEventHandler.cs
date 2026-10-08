

using Microsoft.Extensions.Logging;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> _logger) :
        INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderUpdatedEventHandler received OrderUpdatedEvent for OrderId: {OrderId}", notification.order.Id);
            return Task.CompletedTask;
        }
    }
}
