using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;
using OrderService.Domain.Entities;

namespace OrderService.DataAccess.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable(nameof(OrderItem));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductName)
                .IsRequired()
                .HasMaxLength(DomainConstants.Order.ProductNameMaxLength);

            builder.Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            builder.Property(i => i.TotalPrice)
                .HasPrecision(18, 2);

            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.OrderId).IsRequired();
            builder.Property(i => i.ProductId).IsRequired();
        }
    }
}
