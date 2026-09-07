using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;
using OrderService.Domain.Entities;

namespace OrderService.DataAccess.Configuration
{
    public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable(nameof(InboxMessage));

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Consumer)
                .IsRequired()
                .HasMaxLength(DomainConstants.Message.ConsumerMaxLength);

            builder.Property(m => m.OccurredOn).IsRequired();
            builder.Property(m => m.ProcessedOn);
        }
    }
}
