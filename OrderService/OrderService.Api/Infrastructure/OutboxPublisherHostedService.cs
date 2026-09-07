using OrderService.Api.Infrastructure;

namespace OrderService.Api.Infrastructure
{
    internal sealed class OutboxPublisherHostedService(IMessagePublisher messagePublisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await messagePublisher.ExecuteAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
