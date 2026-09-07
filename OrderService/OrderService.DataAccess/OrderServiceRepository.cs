using BuildingBlocks.DataAccess;
using BuildingBlocks.Domain;

namespace OrderService.DataAccess
{
    public class OrderServiceRepository<TEntity>(OrderServiceEFContext context) : Repository<TEntity>(context)
        where TEntity : class, IEntity;
}
