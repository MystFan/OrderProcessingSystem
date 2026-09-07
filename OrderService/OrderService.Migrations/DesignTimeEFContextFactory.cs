using BuildingBlocks.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrderService.DataAccess;

namespace OrderService.Migrations;

public class DesignTimeEFContextFactory : IDesignTimeDbContextFactory<OrderServiceEFContext>
{  
    public static OrderServiceEFContext CreateDbContext()
    {
        DatabaseOptions options = StaticSettings.DatabaseOptions;
        string connectionString = options.ConnectionString;
        
        var optionsBuilder = new DbContextOptionsBuilder<OrderServiceEFContext>();
        optionsBuilder.UseNpgsql(connectionString, builder =>
        {
            builder.MigrationsAssembly(typeof(DesignTimeEFContextFactory).Assembly.FullName);

            if (StaticSettings.DatabaseOptions.SqlCommandTimeout is > 0)
            {
                builder.CommandTimeout(StaticSettings.DatabaseOptions.SqlCommandTimeout);
            }
        });

        return new OrderServiceEFContext(optionsBuilder.Options, null);
    }

    public OrderServiceEFContext CreateDbContext(string[] args)
    {
        return DesignTimeEFContextFactory.CreateDbContext();
    }
}