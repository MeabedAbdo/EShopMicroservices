using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandler(IApplicationDBContext _dbcontext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerQueryResult>
    {
        public async Task<GetOrdersByCustomerQueryResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            List<Order> orders = await _dbcontext.Orders.Include(o => o.OrderItems)
                .AsNoTracking().Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);
            return new GetOrdersByCustomerQueryResult(orders.MapToOrderDTO());
        }
    }
}
