namespace OrderService.Application.Commands.CreateOrderCommand
{
    public record CreateOrderItemRequest
    {
        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
