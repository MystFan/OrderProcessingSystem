namespace BuildingBlocks.Infrastructure
{
    public class RabbitMQOptions
    {
        public const string SectionName = "rabbitmq";

        public string Host { get; init; } = null!;

        public string Username { get; init; } = null!;

        public string Password { get; init; } = null!;
    }
}
