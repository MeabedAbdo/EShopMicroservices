

using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(OrderId orderId) : base($"Order", orderId)
        {
        }
    }
}
