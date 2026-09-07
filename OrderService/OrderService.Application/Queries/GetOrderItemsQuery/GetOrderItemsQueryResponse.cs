using BuildingBlocks.Application.Abstract.Query;

namespace OrderService.Application.Queries.GetOrderItemsQuery
{
    public record GetOrderItemsQueryResponse : IQueryResponse
    {
        public List<GetOrderItemQueryResponse> OrderItems { get; init; } = [];
    }
}
