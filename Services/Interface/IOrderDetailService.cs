using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;

namespace Services.Interface;

public interface IOrderDetailService
{
    Task<OrderDetail?> AddAsync(OrderDetailRequestDto entity);
    Task<OrderDetail?> UpdateAsync(int id, OrderDetailRequestDto entity);
    Task<int> DeleteAsync(int id);
    Task<OrderDetail?> GetByIdAsync(int id);
    Task<IEnumerable<OrderDetail>?> GetAllAsync();
}