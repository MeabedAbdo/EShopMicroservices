using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers
{
    public class BasketCheckoutEventHandler(ISender _sender, ILogger<BasketCheckoutEventHandler> _logger) :
        IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            _logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
            CreateOrderCommand command = MapEventToCommand(context.Message);
            await _sender.Send(command, CancellationToken.None);
        }
        public CreateOrderCommand MapEventToCommand(BasketCheckoutEvent evt)
        {
            var AddressDto = new AddressDTO(evt.FirstName, evt.LastName, evt.EmailAddress, evt.AddressLine, evt.Country, evt.State, evt.ZipCode);
            var paymentDto = new PaymentDTO(evt.CardName, evt.CardNumber, evt.Expiration, evt.Cvv, evt.PaymentMethod);
            Guid orderId = Guid.NewGuid();
            var orderDto = new OrderDTO(orderId
                , evt.CustomerId
                , evt.UserName,
                AddressDto,
                AddressDto, 
                paymentDto,
                OrderStatus.Draft,
                 OrderItems:
            [
                new OrderItemDTO(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                new OrderItemDTO(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
            ]);
            return new CreateOrderCommand(orderDto);
        }
    }
}
