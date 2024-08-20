using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTests.DAOsTests;

public class OrderDaoTest
{
    private readonly Mock<DbSet<Order>> _mockSet;
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly List<Order> _orderList;

    public OrderDaoTest()
    {
        _mockSet = new Mock<DbSet<Order>>();
        _mockContext = new Mock<ApplicationDbContext>();
        _orderList = new List<Order>
        {
            new Order
            {
                OrderId = 2,
                OrderDate = DateTime.Now.AddDays(-10),
                MemberId = 2,
                ShippedDate = DateTime.Now.AddDays(-8),
                Freight = "15",
                RequiredDate = DateTime.Now.AddDays(-5)
            },
            new Order
            {
                OrderId = 3,
                OrderDate = DateTime.Now.AddDays(-20),
                MemberId = 3,
                ShippedDate = DateTime.Now.AddDays(-18),
                Freight = "20",
                RequiredDate = DateTime.Now.AddDays(-15)
            },
            new Order
            {
                OrderId = 4,
                OrderDate = DateTime.Now.AddDays(-30),
                MemberId = 4,
                ShippedDate = DateTime.Now.AddDays(-28),
                Freight = "25",
                RequiredDate = DateTime.Now.AddDays(-25)
            }

        };
        var queryable = _orderList.AsQueryable();
        _mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(queryable.Provider);
        _mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(queryable.Expression);
        _mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        _mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        _mockSet.As<IAsyncEnumerable<Order>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Order>(queryable.GetEnumerator()));

        _mockContext.Setup(c => c.Orders).Returns(_mockSet.Object);

        // Delete method
        _mockSet.Setup(m => m.Remove(It.IsAny<Order>())).Callback<Order>(m => _orderList.Remove(m));
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => 1);

        // FindAsync method
        _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((object[] ids) => _orderList.SingleOrDefault(a => a.OrderId == (int)ids[0]));

        // Update method
        _mockSet.Setup(m => m.Update(It.IsAny<Order>())).Callback<Order>(updatedOrder =>
        {
            var existingOrder = _orderList.SingleOrDefault(a => a.OrderId == updatedOrder.OrderId);
            if (existingOrder != null)
            {
                existingOrder.OrderDate = updatedOrder.OrderDate;
                existingOrder.MemberId = updatedOrder.MemberId;
                existingOrder.ShippedDate = updatedOrder.ShippedDate;
                existingOrder.Freight = updatedOrder.Freight;
                existingOrder.RequiredDate = updatedOrder.RequiredDate;
            }
        });
    }
    
    [Fact]
    public async Task GetOrders_ReturnsAllOrders()
    {
        var orderDao = new OrderDao(_mockContext.Object);
        var result = await orderDao.GetOrders();
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetOrderById_ReturnsOrder()
    {
        var orderDao = new OrderDao(_mockContext.Object);
        var result = await orderDao.GetOrderById(2);
    }
    
    [Fact]
    public async Task AddOrder_AddsOrder()
    {
        var newOrder = new Order
        {
            OrderId = 5,
            OrderDate = DateTime.Now.AddDays(-40),
            MemberId = 5,
            ShippedDate = DateTime.Now.AddDays(-38),
            Freight = "30",
            RequiredDate = DateTime.Now.AddDays(-35)
        };
        var orderDao = new OrderDao(_mockContext.Object);
        var result = await orderDao.AddOrder(newOrder);
        Assert.Equal(5, result.OrderId);
    }
    
    [Fact]
    public async Task UpdateOrder_UpdatesOrder()
    {
        var updatedOrder = new Order
        {
            OrderId = 2,
            OrderDate = DateTime.Now.AddDays(-10),
            MemberId = 2,
            ShippedDate = DateTime.Now.AddDays(-8),
            Freight = "15",
            RequiredDate = DateTime.Now.AddDays(-5)
        };
        var orderDao = new OrderDao(_mockContext.Object);
        var result = await orderDao.UpdateOrder(2, updatedOrder);
        Assert.Equal(2, result.OrderId);
        Assert.Equal("15", result.Freight);
    }
    
    [Fact]
    public async Task DeleteOrder_DeletesOrder()
    {
        var orderDao = new OrderDao(_mockContext.Object);
        await orderDao.DeleteOrder(2);
        Assert.Equal(2, _orderList.Count);
    }
    
    
}