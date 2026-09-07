namespace BuildingBlocks.DataAccess
{
    public class DatabaseOptions
    {
        public const string SectionName = "database";

        public int? SqlCommandTimeout { get; init; } = 3600;

        public string ConnectionString { get; init; } = null!;
    }
}
