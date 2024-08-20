using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Moq;
using Repositories.Interface;
using Services.Implementation;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class OrderServiceTest
    {
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderService _orderService;
        private readonly List<Order> _orderList;

        public OrderServiceTest()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(_mockRepository.Object, _mockMapper.Object);

            _orderList = new List<Order>
            {
                new Order { OrderId = 1, OrderDate = DateTime.Now, MemberId = 1 },
                new Order { OrderId = 2, OrderDate = DateTime.Now, MemberId = 2 }
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_orderList);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            Assert.Equal(_orderList.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrder()
        {
            // Arrange
            var order = _orderList[0];
            _mockRepository.Setup(repo => repo.GetByIdAsync(order.OrderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(order.OrderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
        }

        [Fact]
        public async Task AddAsync_AddsOrder()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { MemberId = 3 };
            var order = new Order { OrderId = 3, MemberId = orderRequest.MemberId };
            
            _mockMapper.Setup(m => m.Map<Order>(orderRequest)).Returns(order);
            _mockRepository.Setup(repo => repo.AddAsync(order)).ReturnsAsync(order);

            // Act
            var result = await _orderService.AddAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.MemberId, result.MemberId);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesOrder()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { MemberId = 1 };
            var updatedOrder = new Order { OrderId = 1, MemberId = orderRequest.MemberId };

            _mockMapper.Setup(m => m.Map<Order>(orderRequest)).Returns(updatedOrder);
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedOrder.OrderId, updatedOrder)).ReturnsAsync(updatedOrder);

            // Act
            var result = await _orderService.UpdateAsync(updatedOrder.OrderId, orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedOrder.MemberId, result.MemberId);
        }

        [Fact]
        public async Task DeleteAsync_DeletesOrder()
        {
            // Arrange
            var orderId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(orderId)).ReturnsAsync(1);

            // Act
            var result = await _orderService.DeleteAsync(orderId);

            // Assert
            Assert.Equal(1, result);
        }
    }
}