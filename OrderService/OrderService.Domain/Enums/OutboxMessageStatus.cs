namespace OrderService.Domain.Enums
{
    public enum OutboxMessageStatus
    {
        Pending,
        Published,
        Failed
    }
}
