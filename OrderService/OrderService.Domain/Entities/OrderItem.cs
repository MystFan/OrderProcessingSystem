using BuildingBlocks.Domain;

namespace OrderService.Domain.Entities
{
    public class OrderItem : EntityBase<Guid>
    {
        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; } = null!;

        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public decimal TotalPrice { get; private set; }

        // For ORM
        private OrderItem() { }

        public static OrderItem Create(Guid orderId, Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("orderId");
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentException("productId");
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("productName");
            }

            if (unitPrice < 0m)
            {
                throw new ArgumentException("unitPrice");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("quantity");
            }

            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity,
                TotalPrice = unitPrice * quantity
            };

            return item;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("newQuantity");
            }
            Quantity = newQuantity;
            TotalPrice = UnitPrice * Quantity;
        }
    }
}
