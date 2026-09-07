using BuildingBlocks.Application.Abstract.Query;

namespace OrderService.Application.Queries.GetOrderItemsQuery
{
    public record GetOrderItemsQuery : IQuery<GetOrderItemsQueryResponse>
    {
        public Guid OrderId { get; init; }
    }
}
