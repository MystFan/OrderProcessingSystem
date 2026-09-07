using MediatR;

namespace BuildingBlocks.Application.Abstract.Query;

public interface IQueryHandler<in TQuery, TQueryResponse> : IRequestHandler<TQuery, TQueryResponse>
    where TQuery : IQuery<TQueryResponse>
    where TQueryResponse : IQueryResponse
{
    //
}