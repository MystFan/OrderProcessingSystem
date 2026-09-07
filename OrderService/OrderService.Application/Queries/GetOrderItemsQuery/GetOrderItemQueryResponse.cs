namespace OrderService.Application.Queries.GetOrderItemsQuery
{
    public record GetOrderItemQueryResponse
    {
        public Guid ProductId { get; init; }

        public string ProductName { get; init; } = null!;

        public decimal UnitPrice { get; init; }

        public int Quantity { get; init; }

        public decimal TotalPrice { get; init; }
    }
}
