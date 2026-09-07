using BuildingBlocks.Domain;
using System.Linq.Expressions;

namespace BuildingBlocks.Application.Abstract;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    ValueTask CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    IQueryable<TEntity> GetAll();

    IQueryable<TEntity> GetAllBy(Expression<Func<TEntity, bool>> predicate);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
