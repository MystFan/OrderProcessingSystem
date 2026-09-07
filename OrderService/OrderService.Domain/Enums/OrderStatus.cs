namespace OrderService.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        InventoryReserved,
        PaymentSucceeded,
        ShippingCreated,
        Completed,
        Cancelled
    }
}
