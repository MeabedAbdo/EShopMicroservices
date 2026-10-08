using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.DTOs;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Exceptions;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDBContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            OrderId orderId = OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders
                .FindAsync([orderId], cancellationToken);

            if (order is null)
            {
                throw new OrderNotFoundException(orderId);
            }
            UpdateOrderNewValues(order, command.Order);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }
        private void  UpdateOrderNewValues(Order order, OrderDTO dto)
        {
            var shippingAddress = Address.Of(
                dto.ShippingAddress.FirstName,
                dto.ShippingAddress.LastName,
                dto.ShippingAddress.EmailAddress ?? string.Empty,
                dto.ShippingAddress.AddressLine,
                dto.ShippingAddress.Country,
                dto.ShippingAddress.State,
                dto.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(
                dto.BillingAddress.FirstName,
                dto.BillingAddress.LastName,
                dto.BillingAddress.EmailAddress ?? string.Empty,
                dto.BillingAddress.AddressLine,
                dto.BillingAddress.Country,
                dto.BillingAddress.State,
                dto.BillingAddress.ZipCode);
            var payment = Payment.Of(
                dto.Payment.CardName ?? string.Empty,
                dto.Payment.CardNumber,
                dto.Payment.Expiration,
                dto.Payment.Cvv,
                dto.Payment.PaymentMethod);
            order.Update(
                OrderName.Of(dto.OrderName),
                shippingAddress,
                billingAddress,
                payment,
                dto.Status);
            // remove existing items
            var existingItems = order.OrderItems.ToList();
            foreach (var it in existingItems)
            {
                order.Remove(it.ProductId);
            }

            // add new items from DTO
            foreach (var item in dto.OrderItems)
            {
                order.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
            }
        }
    }
}
