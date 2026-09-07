using Moq;
using BuildingBlocks.Application.Abstract;
using OrderService.Application.Queries.GetOrderItemsQuery;
using OrderService.Domain.Entities;
using BuildingBlocks.Tests;

namespace OrderService.Tests.Queries;

public class GetOrderItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsOrderItemsList()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var item1 = OrderItem.Create(orderId, Guid.NewGuid(), "Prod A", 5m, 1);
        var item2 = OrderItem.Create(orderId, Guid.NewGuid(), "Prod B", 3m, 2);

        var data = new OrderItem[] { item1, item2 };
        var mockRepo = new Mock<IRepository<OrderItem>>();
        mockRepo.Setup(r => r.GetAll()).Returns(data.AsAsyncQueryable());

        var handler = new GetOrderItemsQueryHandler(mockRepo.Object);
        var query = new GetOrderItemsQuery { OrderId = orderId };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.OrderItems.Count);
        Assert.Contains(result.OrderItems, i => i.ProductId == item1.ProductId && i.ProductName == item1.ProductName);
        Assert.Contains(result.OrderItems, i => i.ProductId == item2.ProductId && i.ProductName == item2.ProductName);
    }
}