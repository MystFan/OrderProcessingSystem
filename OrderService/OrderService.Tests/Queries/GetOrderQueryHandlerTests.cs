using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Tests;
using Moq;
using OrderService.Application.Queries.GetOrderQuery;
using OrderService.Domain.Entities;
using OrderService.Domain.Exceptions;

namespace OrderService.Tests.Queries;

public class GetOrderQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenOrderExists_ReturnsResponse()
    {
        // Arrange
        var order = Order.Create("cust-1", "USD", "ORD-123");
        order.AddItem(Guid.NewGuid(), "Product A", 10m, 2);

        var data = new[] { order };
        var mockRepo = new Mock<IRepository<Order>>();
        mockRepo.Setup(r => r.GetAll()).Returns(data.AsAsyncQueryable());

        var handler = new GetOrderQueryHandler(mockRepo.Object);
        var query = new GetOrderQuery { OrderId = order.Id };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(order.OrderNumber, result.OrderNumber);
        Assert.Equal(order.CustomerId, result.CustomerId);
        Assert.Equal(order.Status, result.Status);
        Assert.Equal(order.TotalAmount, result.TotalAmount);
        Assert.Equal(order.Currency, result.Currency);
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ThrowsOrderServiceDomainException()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Order>>();
        mockRepo.Setup(r => r.GetAll()).Returns(Array.Empty<Order>().AsAsyncQueryable());

        var handler = new GetOrderQueryHandler(mockRepo.Object);
        var query = new GetOrderQuery { OrderId = Guid.NewGuid() };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<OrderServiceDomainException>(() => handler.Handle(query, CancellationToken.None));
        Assert.Equal(ExceptionReasonCode.OrderNotFound, ex.ReasonCode);
    }
}