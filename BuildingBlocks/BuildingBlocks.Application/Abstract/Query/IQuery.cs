using MediatR;

namespace BuildingBlocks.Application.Abstract.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : IQueryResponse
{
    //
}