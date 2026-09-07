namespace BuildingBlocks.Application.Abstract;

public interface IUnitOfWork
{
    /// <summary>
    /// Persists changes made in the current unit of work and returns the number of affected entities.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of affected entities.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
