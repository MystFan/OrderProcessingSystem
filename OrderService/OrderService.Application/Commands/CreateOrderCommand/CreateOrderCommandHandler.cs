using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Application.Abstract.Command;
using BuildingBlocks.Messaging;
using OrderService.Domain.Entities;
using System.Text.Json;

namespace OrderService.Application.Commands.CreateOrderCommand
{
    public sealed class CreateOrderCommandHandler(IRepository<Order> orderRepository, IRepository<OutboxMessage> outboxMessageRepository) : ICommandHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = Order.Create(request.CustomerId, "EUR");

            foreach (var item in request.Items)
            {
                order.AddItem(Guid.Parse(item.ProductId), item.ProductName, item.UnitPrice, item.Quantity);
            }

            await orderRepository.CreateAsync(order);

            var orderCreated = new OrderCreated
            {
                OrderId = order.Id.ToString(),
                CustomerId = order.CustomerId.ToString(),
                TotalAmount = order.TotalAmount,
                Currency = order.Currency
            };

            var envelope = new MessageEnvelope<OrderCreated>(new MessageMetadata(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), orderCreated);

            var outboxMessage = OutboxMessage.Create(nameof(OrderCreated), JsonSerializer.Serialize(envelope));
            await outboxMessageRepository.CreateAsync(outboxMessage);

            return new CreateOrderCommandResponse(order.Id.ToString());
        }
    }
}
