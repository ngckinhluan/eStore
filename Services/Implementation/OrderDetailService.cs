using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation;

public class OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper) : IOrderDetailService
{
    private IOrderDetailRepository OrderDetailRepository { get; set; } = orderDetailRepository;
    private IMapper Mapper { get; set; } = mapper;

    public Task<OrderDetail?> AddAsync(OrderDetailRequestDto entity)
    {
        var orderDetail = Mapper.Map<OrderDetail>(entity);
        return OrderDetailRepository.AddAsync(orderDetail);
    }

    public Task<OrderDetail?> UpdateAsync(int id, OrderDetailRequestDto entity)
    {
        var orderDetail = Mapper.Map<OrderDetail>(entity);
        return OrderDetailRepository.UpdateAsync(id, orderDetail);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await OrderDetailRepository.DeleteAsync(id);
    }

    public async Task<OrderDetail?> GetByIdAsync(int id)
    {
        return await OrderDetailRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<OrderDetail>?> GetAllAsync()
    {
        return await OrderDetailRepository.GetAllAsync();
    }
}