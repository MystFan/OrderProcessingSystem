using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.DataAccess;
using System.Text.Json;
using BuildingBlocks.Messaging;

namespace OrderService.Api.Infrastructure
{
    public sealed class MessagePublisher(IServiceScopeFactory scopeFactory, ILogger<MessagePublisher> logger) : IMessagePublisher
    {
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceEFContext>();
                IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var messages = await dbContext.OutboxMessages
                    .Where(x => x.PublishedOn == null)
                    .OrderBy(x => x.OccurredOn)
                    .Take(100)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    await PublishAsync(message, publishEndpoint, stoppingToken);

                    message.MarkPublished(DateTime.UtcNow);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing outbox messages");
            }
        }

        private async Task PublishAsync(OutboxMessage message, IPublishEndpoint publishEndpoint, CancellationToken cancellationToken)
        {
            if (message.EventType == nameof(OrderCreated))
            {
                var orderCreated = JsonSerializer.Deserialize<MessageEnvelope<OrderCreated>>(message.Payload);
                if (orderCreated != null)
                {
                    await publishEndpoint.Publish(orderCreated, cancellationToken);
                }
            }
        }
    }
}
