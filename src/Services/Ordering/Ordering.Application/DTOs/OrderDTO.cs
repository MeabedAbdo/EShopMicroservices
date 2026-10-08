using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.DTOs
{
    public record OrderDTO(
        Guid Id,
        Guid CustomerId,
        string OrderName,
        AddressDTO ShippingAddress,
        AddressDTO BillingAddress,
        PaymentDTO Payment,
        OrderStatus Status,
        List<OrderItemDTO> OrderItems
        );
    
}
