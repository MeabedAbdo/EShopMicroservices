using System;
using System.Collections.Generic;
using System.Linq;
using Ordering.Application.DTOs;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions
{
    public static class OrderExtension
    {
        public static List<OrderDTO> MapToOrderDTO(this IEnumerable<Order> orders)
        {
            if (orders is null) return new List<OrderDTO>();

            return orders.Select(order => new OrderDTO(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDTO(
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress,
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State,
                    order.ShippingAddress.ZipCode),
                new AddressDTO(
                    order.BillingAddress.FirstName,
                    order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress,
                    order.BillingAddress.AddressLine,
                    order.BillingAddress.Country,
                    order.BillingAddress.State,
                    order.BillingAddress.ZipCode),
                new PaymentDTO(
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.Cvv,
                    order.Payment.PaymentMethod),
                order.Status,
                order.OrderItems.Select(i => new OrderItemDTO(
                    order.Id.Value,
                    i.ProductId.Value,
                    i.Quantity,
                    i.Price)).ToList()
            )).ToList();
        }
    }
}
