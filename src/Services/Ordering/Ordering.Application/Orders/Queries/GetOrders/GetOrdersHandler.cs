using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDBContext _dbcontext) : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
    {
        public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            int pageIndex = query.PaginatedRequest.PageIndex;
            int pageSize = query.PaginatedRequest.PageSize;
            long totalCount = await _dbcontext.Orders.LongCountAsync(cancellationToken);
            List<Order> orders = await _dbcontext.Orders.Include(o => o.OrderItems)
                                                .AsNoTracking()
                                                .Skip(pageSize * pageIndex)
                                                .Take(pageSize)
                                                .OrderBy(o => o.OrderName.Value)
                                                .ToListAsync();
            return new GetOrdersQueryResult(new PaginatedResult<OrderDTO>( pageIndex, pageSize, totalCount, orders.MapToOrderDTO()));
        }
    }
}
