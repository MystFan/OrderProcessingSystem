using BuildingBlocks.Application.Abstract.Command;

namespace OrderService.Application.Commands.CreateOrderCommand
{
    public record CreateOrderCommand : ICommand<CreateOrderCommandResponse>
    {
        public string CustomerId { get; set; } = null!;

        public CreateOrderItemRequest[] Items { get; set; } = [];
    }
}
