using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.DataAccess;

public static class StaticSettings
{
    public static DatabaseOptions DatabaseOptions { get; } = new();

    static StaticSettings()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        configuration.GetSection("database").Bind(DatabaseOptions);
    }
}