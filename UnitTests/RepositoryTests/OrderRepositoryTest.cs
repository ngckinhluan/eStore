using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Implementation;
using Xunit;

namespace UnitTests.RepositoryTests
{
    public class OrderRepositoryTest
    {
        private readonly Mock<DbSet<Order>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly List<Order> _orderList;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryTest()
        {
            _mockSet = new Mock<DbSet<Order>>();
            _mockContext = new Mock<ApplicationDbContext>();
            _orderList = new List<Order>
            {
                new Order { OrderId = 1, OrderDate = DateTime.Now.AddDays(-5), MemberId = 1, ShippedDate = DateTime.Now.AddDays(-3), Freight = "10", RequiredDate = DateTime.Now.AddDays(-1) },
                new Order { OrderId = 2, OrderDate = DateTime.Now.AddDays(-10), MemberId = 2, ShippedDate = DateTime.Now.AddDays(-8), Freight = "15", RequiredDate = DateTime.Now.AddDays(-5) },
                new Order { OrderId = 3, OrderDate = DateTime.Now.AddDays(-20), MemberId = 3, ShippedDate = DateTime.Now.AddDays(-18), Freight = "20", RequiredDate = DateTime.Now.AddDays(-15) }
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
            var orderDao = new OrderDao(_mockContext.Object);
            _orderRepository = new OrderRepository(orderDao);

            // Delete method
            _mockSet.Setup(m => m.Remove(It.IsAny<Order>())).Callback<Order>(m => _orderList.Remove(m));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

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

            // Add method
            _mockSet.Setup(m => m.Add(It.IsAny<Order>())).Callback<Order>(m => _orderList.Add(m));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
            var result = await _orderRepository.GetAllAsync();
            Assert.Equal(_orderList.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrder()
        {
            var result = await _orderRepository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
        }

        [Fact]
        public async Task AddAsync_AddsOrder()
        {
            // Arrange
            var newOrder = new Order
            {
                OrderId = 4,
                OrderDate = DateTime.Now.AddDays(-40),
                MemberId = 4,
                ShippedDate = DateTime.Now.AddDays(-38),
                Freight = "30",
                RequiredDate = DateTime.Now.AddDays(-35)
            };

            // Act
            await _orderRepository.AddAsync(newOrder);

            // Assert
            _mockSet.Verify(m => m.AddAsync(newOrder, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesOrder()
        {
            // Arrange
            var updatedOrder = new Order
            {
                OrderId = 2,
                OrderDate = DateTime.Now.AddDays(-15),
                MemberId = 2,
                ShippedDate = DateTime.Now.AddDays(-13),
                Freight = "18",
                RequiredDate = DateTime.Now.AddDays(-10)
            };

            // Act
            await _orderRepository.UpdateAsync(updatedOrder.OrderId, updatedOrder);

            // Assert
            var result = await _orderRepository.GetByIdAsync(2);
            Assert.NotNull(result);
            Assert.Equal(updatedOrder.Freight, result.Freight);
        }

        [Fact]
        public async Task DeleteAsync_DeletesOrder()
        {
            // Act
            var result = await _orderRepository.DeleteAsync(1);

            // Assert
            Assert.Equal(1, result); // Verify SaveChangesAsync is called
            Assert.DoesNotContain(_orderList, o => o.OrderId == 1);
        }
    }
}