using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDBContext _dbcontext) :
        IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameQueryResult>
    {
        public async Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            List<Order> orders = await _dbcontext.Orders
                                    .Include(o => o.OrderItems)
                                    .AsNoTracking()
                                    .Where(o => o.OrderName.Value.Contains(query.Name))
                                    .OrderBy(o => o.OrderName.Value)
                                    .ToListAsync(cancellationToken);
            List<OrderDTO> orderDTOs = orders.MapToOrderDTO();
            return new GetOrdersByNameQueryResult(orderDTOs);
        }

    }
}
