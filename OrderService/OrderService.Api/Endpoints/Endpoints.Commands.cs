using BuildingBlocks.Application.Abstract.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands.CreateOrderCommand;

namespace OrderService.Api.Endpoints
{
    public static partial class Endpoints
    {
        private static void MapCommands(IEndpointRouteBuilder app)
        {
            app.MapPost("/create-order", InvokeCommandAsync<CreateOrderCommand, CreateOrderCommandResponse>())
                .WithTags(OpenApiTag);
            
            return;

            Delegate InvokeCommandAsync<TCommand, TCommandResponse>()
                where TCommand : ICommand<TCommandResponse>
                where TCommandResponse : ICommandResponse
            {
                return ([FromBody] TCommand command,
                    [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) => mediator.Send(command, cancellationToken);
            }
        }
    }
}
