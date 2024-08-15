using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation;

public class OrderService(IOrderRepository orderRepository, IMapper mapper) : IOrderService
{
    private IOrderRepository OrderRepository { get; } = orderRepository;
    private IMapper Mapper { get; } = mapper;
    public Task<Order?> AddAsync(OrderRequestDto entity)
    {
        var order = Mapper.Map<Order>(entity);
        return OrderRepository.AddAsync(order);
    }

    public Task<Order?> UpdateAsync(int id, OrderRequestDto entity)
    {
        var order = Mapper.Map<Order>(entity);
        return OrderRepository.UpdateAsync(id, order);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await OrderRepository.DeleteAsync(id);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await OrderRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Order>?> GetAllAsync()
    {
        return await OrderRepository.GetAllAsync();
    }
}