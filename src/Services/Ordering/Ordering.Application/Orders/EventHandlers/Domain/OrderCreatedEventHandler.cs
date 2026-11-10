using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> _logger, IFeatureManager featureManager, IPublishEndpoint _publishEndpoint) :
        INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {

            _logger.LogInformation("OrderCreatedEventHandler received OrderCreatedEvent for OrderId: {OrderId}", domainEvent.order.Id);
            if (await featureManager.IsEnabledAsync("OrderFullfillment"))
            {
                var orderCreatedIntegrationEvent = domainEvent.order.MapToOrderDTO();
                await _publishEndpoint.Publish<OrderDTO>(orderCreatedIntegrationEvent, cancellationToken);
            }

        }
    }
}
