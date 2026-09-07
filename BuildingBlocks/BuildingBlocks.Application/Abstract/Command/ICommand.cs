using MediatR;

namespace BuildingBlocks.Application.Abstract.Command;

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : ICommandResponse
{
    //
}