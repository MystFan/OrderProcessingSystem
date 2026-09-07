using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Application.Abstract.Query;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Application.Queries.GetOrderItemsQuery
{
    public sealed class GetOrderItemsQueryHandler(IRepository<OrderItem> orderItemRepository) : IQueryHandler<GetOrderItemsQuery, GetOrderItemsQueryResponse>
    {
        public async Task<GetOrderItemsQueryResponse> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var orderItems = await orderItemRepository.GetAll().Where(o => o.OrderId == request.OrderId).ToListAsync(cancellationToken);

            return new GetOrderItemsQueryResponse
            {
                OrderItems = orderItems.Select(oi => new GetOrderItemQueryResponse
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };
        }
    }
}
