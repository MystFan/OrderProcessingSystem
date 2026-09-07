namespace BuildingBlocks.Application.Abstract.Command;

public interface IApplicationCommandHandler<in TCommand, TCommandResponse>
    where TCommand : class, IApplicationCommand<TCommandResponse>
    where TCommandResponse: class, IApplicationCommandResponse
{
    Task<TCommandResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}