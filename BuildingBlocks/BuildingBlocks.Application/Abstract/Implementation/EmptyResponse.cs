using BuildingBlocks.Application.Abstract.Command;

namespace BuildingBlocks.Application.Abstract.Implementation;

public class EmptyResponse : ICommandResponse
{
    public static EmptyResponse Instance { get; } = new();

    private EmptyResponse()
    {
        //
    }
}