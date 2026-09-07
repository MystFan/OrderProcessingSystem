using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;
using OrderService.Domain.Entities;

namespace OrderService.DataAccess.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(DomainConstants.Order.OrderNumberMaxLength);

            builder.Property(o => o.Currency)
                .IsRequired()
                .HasMaxLength(DomainConstants.Order.CurrencyMaxLength);

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasMaxLength(DomainConstants.Order.CustomerIdMaxLength);

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.CreatedAt).IsRequired();
            builder.Property(o => o.UpdatedAt).IsRequired();

            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .IsRequired();
        }
    }
}
