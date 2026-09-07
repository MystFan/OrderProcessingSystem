using BuildingBlocks.Domain;

namespace OrderService.Domain.Entities
{
    public class InboxMessage : EntityBase<Guid>
    {
        public string Consumer { get; private set; } = null!;

        public DateTime OccurredOn { get; private set; }

        public DateTime? ProcessedOn { get; private set; }

        private InboxMessage() { }

        public static InboxMessage Create(Guid id, string consumer, DateTime? occurredOn = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("id");
            }

            if (string.IsNullOrWhiteSpace(consumer))
            {
                throw new ArgumentException("consumer");
            }

            return new InboxMessage
            {
                Id = id,
                Consumer = consumer,
                OccurredOn = occurredOn ?? DateTime.UtcNow,
                ProcessedOn = null
            };
        }

        public void MarkProcessed(DateTime processedOn)
        {
            ProcessedOn = processedOn;
        }
    }
}
