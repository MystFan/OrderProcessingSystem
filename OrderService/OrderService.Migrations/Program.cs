using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess;

namespace OrderService.Migrations
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                await using OrderServiceEFContext dbContext = DesignTimeEFContextFactory.CreateDbContext();
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

            return 0;
        }
    }
}
