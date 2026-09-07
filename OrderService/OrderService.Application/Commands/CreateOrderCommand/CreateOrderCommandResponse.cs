using BuildingBlocks.Application.Abstract.Command;

namespace OrderService.Application.Commands.CreateOrderCommand
{
    public record CreateOrderCommandResponse(string OrderId) : ICommandResponse;
}
