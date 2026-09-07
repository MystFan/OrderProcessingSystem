using BuildingBlocks.Application.Abstract.Query;

namespace OrderService.Application.Queries.GetOrderQuery
{
    public record GetOrderQuery : IQuery<GetOrderQueryResponse>
    {
        public Guid OrderId { get; init; }
    }
}
