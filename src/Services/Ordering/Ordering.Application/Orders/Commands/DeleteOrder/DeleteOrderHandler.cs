
namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDBContext _dbcontext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            OrderId orderId = OrderId.Of(request.OrderId);
            var order = await _dbcontext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException(orderId);
            }
            _dbcontext.Orders.Remove(order);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);
        }
    }
}
