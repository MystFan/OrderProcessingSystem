using BuildingBlocks.Application.Abstract.Command;

namespace BuildingBlocks.Application.Abstract.Implementation;

public class EmptyCommandResponse : IApplicationCommandResponse
{
    public static EmptyCommandResponse Instance { get; } = new();

    private EmptyCommandResponse()
    {
        //
    }
}