namespace OrderService.Api.Infrastructure
{
    public interface IMessagePublisher
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
