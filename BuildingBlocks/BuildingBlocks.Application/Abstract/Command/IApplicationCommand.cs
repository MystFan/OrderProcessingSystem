namespace BuildingBlocks.Application.Abstract.Command;

public interface IApplicationCommand<TResponse> where TResponse : class, IApplicationCommandResponse
{
    //
}