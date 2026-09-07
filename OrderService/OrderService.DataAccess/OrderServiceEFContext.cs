using BuildingBlocks.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderService.Domain.Entities;

namespace OrderService.DataAccess
{
    public class OrderServiceEFContext(DbContextOptions<OrderServiceEFContext> options, IOptions<DatabaseOptions>? databaseOptions) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<InboxMessage> InboxMessages { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (databaseOptions is null)
            {
                return;
            }

            DatabaseOptions options = databaseOptions.Value;

            optionsBuilder.UseNpgsql(options.ConnectionString, builder =>
            {
                if (databaseOptions.Value.SqlCommandTimeout is > 0)
                {
                    builder.CommandTimeout(options.SqlCommandTimeout);
                }
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderServiceEFContext).Assembly);
        }
    }
}
