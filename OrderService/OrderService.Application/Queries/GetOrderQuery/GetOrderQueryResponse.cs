using BuildingBlocks.Application.Abstract.Query;
using OrderService.Domain.Enums;

namespace OrderService.Application.Queries.GetOrderQuery
{
    public record GetOrderQueryResponse : IQueryResponse
    {
        public string OrderNumber { get; init; } = null!;

        public string CustomerId { get; init; } = null!;

        public OrderStatus Status { get; init; }

        public decimal TotalAmount { get; init; }

        public string Currency { get; init; } = null!;

        public DateTime UpdatedAt { get; init; }
    }
}
