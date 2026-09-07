using BuildingBlocks.Domain;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities
{
    public class Order : EntityBase<Guid>
    {
        public string OrderNumber { get; private set; } = null!;

        public string CustomerId { get; private set; } = null!;

        public OrderStatus Status { get; private set; }

        public decimal TotalAmount { get; private set; }

        public string Currency { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        private readonly List<OrderItem> _items = new();

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // For ORM
        private Order() { }

        public static Order Create(string customerId, string currency, string? orderNumber = null)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("customerId");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("currency");
            }

            var now = DateTime.UtcNow;
            return new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = orderNumber ?? GenerateOrderNumber(),
                CustomerId = customerId,
                Status = OrderStatus.Pending,
                TotalAmount = 0m,
                Currency = currency,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            var item = OrderItem.Create(Id, productId, productName, unitPrice, quantity);
            _items.Add(item);
            RecalculateTotal();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveItem(Guid orderItemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == orderItemId);
            if (item == null)
            {
                throw new InvalidOperationException("Order item not found.");
            }
            _items.Remove(item);
            RecalculateTotal();
            UpdatedAt = DateTime.UtcNow;
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(i => i.TotalPrice);
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant()}";
        }

        // Status transitions
        public void MarkInventoryReserved()
        {
            if (Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Invalid status transition.");
            }
            Status = OrderStatus.InventoryReserved;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkPaymentSucceeded()
        {
            if (Status != OrderStatus.InventoryReserved)
            {
                throw new InvalidOperationException("Invalid status transition.");
            }
            Status = OrderStatus.PaymentSucceeded;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkShippingCreated()
        {
            if (Status != OrderStatus.PaymentSucceeded)
            {
                throw new InvalidOperationException("Invalid status transition.");
            }
            Status = OrderStatus.ShippingCreated;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != OrderStatus.ShippingCreated)
            {
                throw new InvalidOperationException("Invalid status transition.");
            }
            Status = OrderStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel a completed order.");
            }
            Status = OrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
