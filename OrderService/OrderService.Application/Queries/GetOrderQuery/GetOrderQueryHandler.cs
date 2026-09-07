using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Application.Abstract.Query;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Exceptions;

namespace OrderService.Application.Queries.GetOrderQuery
{
    public sealed class GetOrderQueryHandler(IRepository<Order> orderRepository) : IQueryHandler<GetOrderQuery, GetOrderQueryResponse>
    {
        public async Task<GetOrderQueryResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetAll().FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new OrderServiceDomainException(ExceptionReasonCode.OrderNotFound, "Order not found");
            }

            return new GetOrderQueryResponse
            {
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                Status = order.Status,
                Currency = order.Currency,
                TotalAmount = order.TotalAmount,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
