using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;
using OrderService.Domain.Entities;

namespace OrderService.DataAccess.Configuration
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable(nameof(OutboxMessage));

            builder.HasKey(m => m.Id);

            builder.Property(m => m.EventType)
                .IsRequired()
                .HasMaxLength(DomainConstants.Message.EventTypeMaxLength);

            builder.Property(m => m.Payload)
                .IsRequired()
                .HasMaxLength(DomainConstants.Message.PayloadMaxLength);

            builder.Property(m => m.OccurredOn).IsRequired();
            builder.Property(m => m.PublishedOn);
            builder.Property(m => m.Status).IsRequired();
        }
    }
}
