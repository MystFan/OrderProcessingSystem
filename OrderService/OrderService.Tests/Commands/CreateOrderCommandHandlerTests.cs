using System.Text.Json;
using Moq;
using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Messaging;
using OrderService.Application.Commands.CreateOrderCommand;
using OrderService.Domain.Entities;

namespace OrderService.Tests.Commands;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesOrderAndOutboxMessage()
    {
        // Arrange
        Order? createdOrder = null;
        OutboxMessage? createdOutbox = null;

        var mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo
            .Setup(r => r.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((o, ct) => createdOrder = o)
            .Returns(ValueTask.CompletedTask);

        var mockOutboxRepo = new Mock<IRepository<OutboxMessage>>();
        mockOutboxRepo
            .Setup(r => r.CreateAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()))
            .Callback<OutboxMessage, CancellationToken>((m, ct) => createdOutbox = m)
            .Returns(ValueTask.CompletedTask);

        var handler = new CreateOrderCommandHandler(mockOrderRepo.Object, mockOutboxRepo.Object);

        var command = new CreateOrderCommand
        {
            CustomerId = "cust-1",
            Items = new[]
            {
                new CreateOrderItemRequest { ProductId = Guid.NewGuid().ToString(), ProductName = "Prod A", UnitPrice = 10m, Quantity = 2 }
            }
        };

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response.OrderId);
        Assert.NotNull(createdOrder);
        Assert.Equal("cust-1", createdOrder!.CustomerId);
        Assert.Single(createdOrder.Items);
        Assert.Equal(20m, createdOrder.TotalAmount);

        Assert.NotNull(createdOutbox);
        Assert.Equal(nameof(OrderCreated), createdOutbox!.EventType);

        var envelope = JsonSerializer.Deserialize<MessageEnvelope<OrderCreated>>(createdOutbox.Payload);
        Assert.NotNull(envelope);
        Assert.Equal(response.OrderId, envelope!.Payload.OrderId);
    }
}
