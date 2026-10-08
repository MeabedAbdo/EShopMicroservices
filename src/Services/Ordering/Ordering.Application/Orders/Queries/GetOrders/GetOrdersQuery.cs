

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(PaginatedRequest PaginatedRequest) : IQuery<GetOrdersQueryResult>;
    public record GetOrdersQueryResult(PaginatedResult<OrderDTO> Orders);
}
