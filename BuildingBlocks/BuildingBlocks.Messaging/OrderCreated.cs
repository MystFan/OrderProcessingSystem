namespace BuildingBlocks.Messaging
{
    public record OrderCreated
    {
        public string OrderId { get; init; } = null!;

        public string CustomerId { get; init; } = null!;

        public decimal TotalAmount { get; init; }

        public string Currency { get; init; } = null!;
    }
}
