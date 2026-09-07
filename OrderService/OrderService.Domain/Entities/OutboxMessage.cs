using BuildingBlocks.Domain;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities
{
    public class OutboxMessage : EntityBase<Guid>
    {
        public string EventType { get; private set; } = null!;

        public string Payload { get; private set; } = null!;

        public DateTime OccurredOn { get; private set; }

        public DateTime? PublishedOn { get; private set; }

        public OutboxMessageStatus Status { get; private set; }

        private OutboxMessage() { }

        public static OutboxMessage Create(string eventType, string payload)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                throw new ArgumentException(nameof(eventType));
            }

            if (payload is null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                Payload = payload,
                OccurredOn = DateTime.UtcNow,
                Status = OutboxMessageStatus.Pending
            };
        }

        public void MarkPublished(DateTime publishedOn)
        {
            PublishedOn = publishedOn;
            Status = OutboxMessageStatus.Published;
        }

        public void MarkFailed()
        {
            Status = OutboxMessageStatus.Failed;
        }
    }
}
